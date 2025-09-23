var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapGet("/cookie-minimal", (HttpContext context) => {
    return Results.Ok(context.Request.Cookies["session-id"]);
});

app.Run();
 