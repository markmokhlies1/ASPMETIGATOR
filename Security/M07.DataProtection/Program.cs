using M07.DataProtection.Data;
using M07.DataProtection.Requests;
using M07.DataProtection.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBiddingService, BiddingService>();

builder.Services.AddDataProtection()
    .PersistKeysToDbContext<AppDbContext>();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});

var app = builder.Build();

app.MapGet("/api/bids", async (IBiddingService biddingService) =>
{
    return Results.Ok(await biddingService.GetAllBidsAsync());
});


app.MapGet("/api/bids/{bidId:guid}", async (Guid bidId, IBiddingService biddingService) =>
{
    var bid = await biddingService.GetBidAsync(bidId);
    if (bid == null)
        return Results.NotFound($"Bid with ID {bidId} not found");

    return Results.Ok(bid);
});

app.MapPost("/api/bids", async (CreateBidRequest request, IBiddingService biddingService) =>
{
    var bid = await biddingService.CreateBidAsync(request);
    return Results.Created($"/api/bids/{bid.Id}", bid);
});

app.Run();


