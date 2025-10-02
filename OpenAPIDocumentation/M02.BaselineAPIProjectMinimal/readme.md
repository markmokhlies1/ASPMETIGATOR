# Step 01 Add XML Comment

```csharp
private static async Task<Ok<ProjectResponse>> GetProject(Guid projectId, IProjectService service)
        => TypedResults.Ok(await service.GetProjectAsync(projectId));


group.MapGet("{projectId:guid}", GetProject)
    .RequireAuthorization(Permission.Project.Read)
    .Produces<ProjectResponse>(StatusCodes.Status200OK)
    .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
    .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
    .WithOpenApi()
    .WithName("GetProjectV1")
    .WithTags("Projects");
```
