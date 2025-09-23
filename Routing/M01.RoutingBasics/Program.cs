var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapGet("/products", () =>{
    return Results.Ok(new [] {
        "Product #1",
        "Product #2"
    });
});


app.MapGet("/route-table", (IServiceProvider sp) =>{
   
    var endpoints = sp.GetRequiredService<EndpointDataSource>()
    .Endpoints.Select(ep=>ep.DisplayName);

    return Results.Ok(endpoints);
});


app.Run();
