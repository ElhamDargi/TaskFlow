using TaskFlowApi.DTOs;
using TaskFlowApi.Models;

namespace TaskFlowApi.Mappers;

public static class ProjectMapper
{
    public static ProjectResponse ToResponse(Project project)
    {
        var response = new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            CreatedAt = project.CreatedAt,
            Description = project.Description
        };
        return response;
    }
}