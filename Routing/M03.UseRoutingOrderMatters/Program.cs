var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();   
 
app.MapGet("/products", () =>{
    return Results.Ok(new [] {
        "Product #1",
        "Product #2"
    });
}); 

app.Run();
 
 