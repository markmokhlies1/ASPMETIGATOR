using M01.ActionFilters.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    // options.Filters.Add<TrackActionTimeFilter>(); // Global Filter Registrations
});

var app = builder.Build();

app.MapControllers();

app.Run();


