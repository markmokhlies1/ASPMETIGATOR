using M02.QueryString.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/product-minimal", (int page, int pageSize) => { 
    return Results.Ok($"Showing {pageSize} items of page # {page}.");
});

app.MapGet("/product-minimal-1", ([FromQuery(Name = "page")] int p, [FromQuery(Name = "pageSize")]int ps) => { 
    return Results.Ok($"Showing {p} items of page # {ps}.");
});
 
app.MapGet("/product-minimal-asparameters", ([AsParameters] SearchRequest request) => {
    return Results.Ok(request);
});

app.MapGet("/product-minimal-array", ([FromQuery] Guid[] ids) => {
    return Results.Ok(ids);
});

app.MapGet("/date-range-minimal", (DateRangeQuery dateRange) => {
    return Results.Ok(dateRange);
});
 
app.MapControllers();

app.Run();

public class SearchRequest
{
    public string? Query {get; set;}
    public int Page {get; set;}
    public int PageSize {get; set;}
}