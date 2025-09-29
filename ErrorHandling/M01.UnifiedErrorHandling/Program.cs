using M01.DefaultBehaviour.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapErrorEndpoints();

app.Run();
