namespace TaskFlowApi.Services;

using Microsoft.EntityFrameworkCore;
using TaskFlowApi.Data;
using TaskFlowApi.DTOs;
using TaskFlowApi.Exceptions;
using TaskFlowApi.Mappers;
using TaskModel = TaskFlowApi.Models.Task;
using TaskStatus = TaskFlowApi.Domain.Enum.TaskStatus;

public class TaskService
{
    private readonly TaskFlowDbContext _context;
    private readonly TaskStatusTransitionValidator _validator;

    public TaskService(TaskFlowDbContext context, TaskStatusTransitionValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<IEnumerable<TaskResponse>> GetAllTasksAsync()
    {
        var tasks = await _context.Tasks.ToListAsync();
        return tasks.Select(TaskMapper.ToResponse);
    }

    public async Task<TaskResponse> GetTaskByIdAsync(Guid taskId)
    {
        var task = await _context.Tasks.FindAsync(taskId) 
            ?? throw new NotFoundException("Task not found");

        return TaskMapper.ToResponse(task);
    }

    public async Task<IEnumerable<TaskResponse>> GetTasksByProjectAsync(Guid projectId)
    {
        var tasks = await _context.Tasks.Where(t => t.ProjectId == projectId).ToListAsync();
        if (tasks.Count == 0) throw new NotFoundException("Not found any tasks for this project");

        return tasks.Select(TaskMapper.ToResponse);
    }

    public async Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request)
    {
        if (request.ProjectId == Guid.Empty)
            throw new ValidationException("Project id is required", "ProjectId");

        var project = await _context.Projects.FindAsync(request.ProjectId)
            ?? throw new NotFoundException("Project not found");

        var newTask = new TaskModel
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            Status = TaskStatus.Todo,
            ProjectId = request.ProjectId,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow,
            Project = project
        };

        _context.Tasks.Add(newTask);
        await _context.SaveChangesAsync();

        return TaskMapper.ToResponse(newTask);
    }

    public async Task<TaskResponse> UpdateTaskAsync(Guid taskId, UpdateTaskRequest request)
    {
        var taskEntity = await _context.Tasks.FindAsync(taskId)
            ?? throw new NotFoundException("Task not found");

        if (!_validator.IsValidTransition(taskEntity.Status, request.Status))
            throw new ValidationException("Transition is invalid", "status", ["Invalid status transition"]);

        if (request.ProjectId == Guid.Empty)
            throw new ValidationException("Project id is required", "ProjectId");

        var project = await _context.Projects.FindAsync(request.ProjectId)
            ?? throw new NotFoundException("Project not found");

        taskEntity.Title = request.Title;
        taskEntity.Description = request.Description;
        taskEntity.Priority = request.Priority;
        taskEntity.Status = request.Status;
        taskEntity.DueDate = request.DueDate;
        taskEntity.ProjectId = request.ProjectId;

        await _context.SaveChangesAsync();

        return TaskMapper.ToResponse(taskEntity);
    }

    public async Task DeleteTaskAsync(Guid taskId)
    {
        var taskEntity = await _context.Tasks.FindAsync(taskId)
            ?? throw new NotFoundException("Task not found");

        _context.Tasks.Remove(taskEntity);
        await _context.SaveChangesAsync();
    }
}