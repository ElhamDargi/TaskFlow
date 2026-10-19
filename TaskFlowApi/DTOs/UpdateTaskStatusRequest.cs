using System.ComponentModel.DataAnnotations;
using TaskStatus = TaskFlowApi.Domain.Enum.TaskStatus;

namespace TaskFlowApi.DTOs;

public class UpdateTaskStatusRequest
{
    [Required]
    [EnumDataType(typeof(TaskStatus))]
    public TaskStatus? Status { get; set; }
}