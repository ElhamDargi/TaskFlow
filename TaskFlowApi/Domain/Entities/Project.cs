namespace TaskFlowApi.Models;

public class Project
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}