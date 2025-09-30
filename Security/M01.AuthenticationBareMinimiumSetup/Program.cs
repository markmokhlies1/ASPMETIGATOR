using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
// })
//     .AddCookie();

// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie();

// builder.Services.AddAuthentication("Cookies")
//     .AddCookie();

builder.Services.AddAuthentication()
    .AddCookie(); // AddJwtBearer, AddOpenId, ....

var app = builder.Build();

app.UseAuthentication();

app.MapGet("/login", async (HttpContext httpContext) =>
{
    List<Claim> claims = [
        new ("name", "Issam A."),
        new ("email", "issam@localhost"),
        new ("sub", Guid.NewGuid().ToString())
    ];

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

    var principal = new ClaimsPrincipal(identity);

    await httpContext.SignInAsync(
        scheme: CookieAuthenticationDefaults.AuthenticationScheme,
        principal: principal
    );
});


app.MapGet("/logout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync();
});

app.MapGet("/user", (HttpContext httpContext) =>
{
    var principal = httpContext.User;

    if (principal.Identity is { IsAuthenticated: true })
    {
        var claims = principal.Claims.Select(c => new { c.Type, c.Value });

        return Results.Ok(claims);
    }

    return Results.Unauthorized();
});

app.Run();
