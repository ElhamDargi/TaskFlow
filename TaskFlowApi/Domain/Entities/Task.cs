using TaskFlowApi.Domain.Enum;
using TaskStatus = TaskFlowApi.Domain.Enum.TaskStatus;

namespace TaskFlowApi.Models;

public class Task
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public required Project Project { get; set; }
    public Guid ProjectId { get; set; }
}