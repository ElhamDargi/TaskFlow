using TaskFlowApi.DTOs;
using Task = TaskFlowApi.Models.Task;

namespace TaskFlowApi.Mappers;

public static class TaskMapper
{
    public static TaskResponse ToResponse(Task task)
    {
        var response = new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority,
            Status = task.Status,
            ProjectId = task.ProjectId,
            CreatedAt = task.CreatedAt,
            DueDate = task.DueDate,
        };
        return response;
    }
}