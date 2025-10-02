using M02.BaselineAPIProjectMinimal.Entities;

namespace M02.BaselineAPIProjectMinimal.Responses;

public class ProjectResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime ExpectedStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Budget { get; set; }

    public string? Currency { get; set; }
    public List<ProjectTaskResponse> Tasks { get; set; } = [];

    public static ProjectResponse FromEntity(Project project) => new()
    {
        Id = project.Id,
        Name = project.Name,
        Description = project.Description,
        ExpectedStartDate = project.ExpectedStartDate,
        ActualEndDate = project.ActualEndDate,
        OwnerId = project.OwnerId,
        CreatedAt = project.CreatedAt,
        Budget = project.Budget,
        Tasks = project.Tasks.Select(ProjectTaskResponse.FromEntity).ToList()
    };
}
