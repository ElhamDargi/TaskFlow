using TaskStatus = TaskFlowApi.Domain.Enum.TaskStatus;

namespace TaskFlowApi.Services;

public class TaskStatusTransitionValidator
{
    public bool IsValidTransition(TaskStatus currentStatus, TaskStatus newStatus)
    {
        return currentStatus == newStatus ||
               (currentStatus == TaskStatus.Todo && newStatus == TaskStatus.InProgress) ||
               (currentStatus == TaskStatus.InProgress && newStatus == TaskStatus.Done);
    }
}