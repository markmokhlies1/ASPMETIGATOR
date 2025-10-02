using System.Text;
using System.Text.Json.Serialization;
using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using M02.BaselineAPIProjectMinimal.Data;
using M02.BaselineAPIProjectMinimal.Exceptions;
using M02.BaselineAPIProjectMinimal.Identity;
using M02.BaselineAPIProjectMinimal.OpenApi.Transformers;
using M02.BaselineAPIProjectMinimal.Permissions;
using M02.BaselineAPIProjectMinimal.Services;
using M02.BaselineAPIProjectMinimal.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace M02.BaselineAPIProjectMinimal;


public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomProblemDetails()
                .AddApiDocumentation()
                .AddCustomApiVersioning()
                .AddExceptionHandling()
                .AddJsonConfiguration()
                .AddValidation()
                .AddDatabase(configuration)
                .AddJwtAuthentication(configuration)
                .AddAuthorizationPolicies()
                .AddBusinessServices();

        return services;
    }

    public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = (context) =>
            {
                context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
            };
        });

        return services;
    }

    public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
    {
        string[] versions = ["v1", "v2"];

        foreach (var version in versions)
        {
            services.AddOpenApi(version, options =>
            {
                // Versioning config
                options.AddDocumentTransformer<VersionInfoTransformer>();

                // Security Scheme config

                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                options.AddOperationTransformer<BearerSecuritySchemeTransformer>();
            });
        }
        return services;
    }

    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        return services;
    }

    public static IServiceCollection AddJsonConfiguration(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateProjectRequestValidator>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source = app.db";

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // TODO: Set to true in production
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)
                )
            };
        });

        return services;
    }

    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Project Management Permissions
            AddProjectPolicies(options);

            // Task Management Permissions
            AddTaskPolicies(options);
        });

        return services;
    }

    private static void AddProjectPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(Permission.Project.Create,
            policy => policy.RequireClaim("permission", Permission.Project.Create));
        options.AddPolicy(Permission.Project.Read,
            policy => policy.RequireClaim("permission", Permission.Project.Read));
        options.AddPolicy(Permission.Project.Update,
            policy => policy.RequireClaim("permission", Permission.Project.Update));
        options.AddPolicy(Permission.Project.Delete,
            policy => policy.RequireClaim("permission", Permission.Project.Delete));
        options.AddPolicy(Permission.Project.ManageBudget,
            policy => policy.RequireClaim("permission", Permission.Project.ManageBudget));
    }

    private static void AddTaskPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(Permission.Task.Create,
            policy => policy.RequireClaim("permission", Permission.Task.Create));
        options.AddPolicy(Permission.Task.Read,
            policy => policy.RequireClaim("permission", Permission.Task.Read));
        options.AddPolicy(Permission.Task.Update,
            policy => policy.RequireClaim("permission", Permission.Task.Update));
        options.AddPolicy(Permission.Task.Delete,
            policy => policy.RequireClaim("permission", Permission.Task.Delete));
        options.AddPolicy(Permission.Task.AssignUser,
            policy => policy.RequireClaim("permission", Permission.Task.AssignUser));
        options.AddPolicy(Permission.Task.UpdateStatus,
            policy => policy.RequireClaim("permission", Permission.Task.UpdateStatus));
        options.AddPolicy(Permission.Task.Comment,
            policy => policy.RequireClaim("permission", Permission.Task.Comment));
    }

    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<JwtTokenProvider>();
        services.AddScoped<IProjectService, ProjectService>();

        return services;
    }
}