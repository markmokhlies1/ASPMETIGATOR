using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/product-minimal", ([FromHeader(Name = "X-Api-Version")] string apiVersion) => { 
    return Results.Ok($"Api version: {apiVersion}");
});

app.MapControllers();

app.Run();
