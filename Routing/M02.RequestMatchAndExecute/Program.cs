var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

#pragma warning disable ASP0014 // Suggest using top level route registrations

app.UseEndpoints(ep=> {
    ep.MapControllers();
    ep.MapGet("/products", () =>{
            return Results.Ok(new [] {
                "Product #1",
                "Product #2"
            });
    });
});
#pragma warning restore ASP0014 // Suggest using top level route registrations

app.Run();
