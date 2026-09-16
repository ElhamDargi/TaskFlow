using System.ComponentModel.DataAnnotations;

namespace TaskFlowApi.DTOs;

public class UpdateProjectRequest
{
    [Required] 
    [MaxLength(100)] 
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)] 
    public string? Description { get; set; }
}