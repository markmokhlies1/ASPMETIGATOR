using System.Threading.RateLimiting;
using M06.RateLimiting.Data;
using M06.RateLimiting.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("DefaultPolicy", limiterOptions =>
    {
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.PermitLimit = 100;
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 10;
    });

    options.AddSlidingWindowLimiter("SlidingWindow", limiterOptions =>
    {
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.PermitLimit = 100;
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 10;
        limiterOptions.SegmentsPerWindow = 6;
        limiterOptions.AutoReplenishment = true;
    });

    options.AddConcurrencyLimiter("Concurrency", limiterOptions =>
    {
        limiterOptions.PermitLimit = 50;
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 100;
    });

    options.AddPolicy("ApiUserPolicy", httpContext =>
           RateLimitPartition.GetFixedWindowLimiter(
               partitionKey: httpContext.User.Identity?.Name ?? "anonymous",
               factory: _ => new FixedWindowRateLimiterOptions
               {
                   Window = TimeSpan.FromMinutes(1),
                   PermitLimit = 1000,
                   AutoReplenishment = true
               }));

    options.AddPolicy("IpPolicy", httpContext =>
          RateLimitPartition.GetSlidingWindowLimiter(
              partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
              factory: _ => new SlidingWindowRateLimiterOptions
              {
                  Window = TimeSpan.FromMinutes(1),
                  PermitLimit = 100,
                  SegmentsPerWindow = 6,
                  AutoReplenishment = true
              }));
});


builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});

var app = builder.Build();

app.UseRateLimiter();

app.MapControllers();

app.MapGet("/api/products-mn", async (IProductService productService, int page = 1, int pageSize = 10) =>
{
    Console.WriteLine("Minimal Endpoint visited");

    var PagedResult = await productService.GetProductsAsync(page, pageSize);

    return Results.Ok(PagedResult);
}).RequireRateLimiting("DefaultPolicy");

app.MapGet("/api/products-mn/{productId:int}", async (
    int productId,
    IProductService productService) =>
{
    Console.WriteLine("Minimal Api (Get By Id) visited");

    var response = await productService.GetProductByIdAsync(productId);

    return response is null
        ? Results.NotFound($"Product with Id '{productId}' not found")
        : Results.Ok(response);
});

app.Run();