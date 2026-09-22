namespace TaskFlowApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using TaskFlowApi.DTOs;
using TaskFlowApi.Services;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly TaskService _taskService;

    public TasksController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetTasks()
    {
        var response = await _taskService.GetAllTasksAsync();
        return Ok(response);
    }

    [HttpGet("{taskId}")]
    public async Task<ActionResult<TaskResponse>> GetTaskById(Guid taskId)
    {
        var response = await _taskService.GetTaskByIdAsync(taskId);
        return Ok(response);
    }

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetTasksByProject(Guid projectId)
    {
        var response = await _taskService.GetTasksByProjectAsync(projectId);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> PostTask(CreateTaskRequest request)
    {
        var response = await _taskService.CreateTaskAsync(request);
        return CreatedAtAction(nameof(GetTaskById), new { taskId = response.Id }, response);
    }

    [HttpPut("{taskId}")]
    public async Task<ActionResult<TaskResponse>> PutTask(Guid taskId, UpdateTaskRequest task)
    {
        var response = await _taskService.UpdateTaskAsync(taskId, task);
        return Ok(response);
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(Guid taskId)
    {
        await _taskService.DeleteTaskAsync(taskId);
        return NoContent();
    }
}