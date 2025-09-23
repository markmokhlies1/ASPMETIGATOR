using M07.ParameterTransformers.Transformers;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddRouting(options => 
    options.ConstraintMap["slugify"] = typeof(SlugifyTransformer)
);
var app = builder.Build();    

// Clean Code A Handbook of Agile Software Craftsmanship
// clean-code-a-handbook-of-agile-software-craftsmanship

app.MapGet("/book/{title:slugify}", (string title) => {
    return Results.Ok(new { title });
}).WithName("BookByTitle");
 
app.MapGet("/generate",(LinkGenerator link, HttpContext context) => {

    var url = link.GetPathByName("BookByTitle", new {
        title = "Clean Code A Handbook of Agile Software Craftsmanship"
    });
    return Results.Ok(new { generatedUrl = url });
});
app.Run();