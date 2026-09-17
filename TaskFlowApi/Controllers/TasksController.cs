using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlowApi.Data;
using TaskFlowApi.DTOs;
using TaskFlowApi.Mappers;
using TaskFlowApi.Services;
using TaskModel = TaskFlowApi.Models.Task;
using TaskStatus = TaskFlowApi.Domain.Enum.TaskStatus;

namespace TaskFlowApi.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly TaskFlowDbContext _context;
    private readonly TaskStatusTransitionValidator _validator;

    public TasksController(TaskFlowDbContext context, TaskStatusTransitionValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetTasks()
    {
        var tasks = await _context.Tasks.ToListAsync();
        var response = tasks.Select(TaskMapper.ToResponse).ToList();
        return Ok(response);
    }

    [HttpGet("{taskId}")]
    public async Task<ActionResult<TaskResponse>> GetTaskById(Guid taskId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null) return NotFound("Task not found");
        var response = TaskMapper.ToResponse(task);
        return Ok(response);
    }

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetTasksByProject(Guid projectId)
    {
        var tasks = await _context.Tasks.Where(t => t.ProjectId == projectId).ToListAsync();
        if (tasks.Count == 0) return NotFound("Not found any tasks for this project");
        var response = tasks.Select(TaskMapper.ToResponse).ToList();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> PostTask(CreateTaskRequest request)
    {
        if (request.ProjectId == Guid.Empty) return BadRequest("Project id is required");
        var project = await _context.Projects.FindAsync(request.ProjectId);
        if (project == null) return NotFound("Project not found");
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
        var response = TaskMapper.ToResponse(newTask);
        return CreatedAtAction(nameof(GetTaskById), new {taskId = response.Id}, response);
    }

    [HttpPut("{taskId}")]
    public async Task<ActionResult<TaskResponse>> PutTask(Guid taskId, UpdateTaskRequest task)
    {
        var taskEntity = await _context.Tasks.FindAsync(taskId);
        if (taskEntity == null) return NotFound("Task not found");
        if (!_validator.IsValidTransition(taskEntity.Status, task.Status))
            return BadRequest("Invalid status transition");
        if (task.ProjectId == Guid.Empty) return BadRequest("Project id is required");
        var project = await _context.Projects.FindAsync(task.ProjectId);
        if (project == null) return NotFound("Project not found");
        taskEntity.Title = task.Title;
        taskEntity.Description = task.Description;
        taskEntity.Priority = task.Priority;
        taskEntity.Status = task.Status;
        taskEntity.DueDate = task.DueDate;
        taskEntity.ProjectId = task.ProjectId;
        await _context.SaveChangesAsync();
        var response = TaskMapper.ToResponse(taskEntity);
        return Ok(response);
    }

    [HttpDelete("{taskId}")]
    public async Task<ActionResult> DeleteTask(Guid taskId)
    {
        var taskEntity = await _context.Tasks.FindAsync(taskId);
        if (taskEntity == null) return NotFound("Task not found");
        _context.Tasks.Remove(taskEntity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}