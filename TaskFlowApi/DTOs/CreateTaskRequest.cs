using System.ComponentModel.DataAnnotations;
using TaskFlowApi.Domain.Enum;

namespace TaskFlowApi.DTOs;

public class CreateTaskRequest
{
    [Required] 
    [MaxLength(100)] 
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.Low;

    public DateTime? DueDate { get; set; }

    public Guid ProjectId { get; set; }
    
}