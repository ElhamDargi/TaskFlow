using System.ComponentModel.DataAnnotations;
using TaskFlowApi.Domain.Enum;
using TaskStatus = TaskFlowApi.Domain.Enum.TaskStatus;

namespace TaskFlowApi.DTOs;

public class UpdateTaskRequest
{
    [Required] 
    [MaxLength(100)] 
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }

    public TaskPriority Priority { get; set; }
    
    public TaskStatus Status { get; set; }

    public DateTime? DueDate { get; set; }

    public Guid ProjectId { get; set; }
}