namespace TaskFlowApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using TaskFlowApi.DTOs;
using TaskFlowApi.Services;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly ProjectService _projectService;

    public ProjectsController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectResponse>>> GetProjects()
    {
        var response = await _projectService.GetAllProjectsAsync();
        return Ok(response);
    }

    [HttpGet("{projectId}")]
    public async Task<ActionResult<ProjectResponse>> GetProject(Guid projectId)
    {
        var response = await _projectService.GetProjectByIdAsync(projectId);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> PostProject(CreateProjectRequest request)
    {
        var response = await _projectService.CreateProjectAsync(request);
        return CreatedAtAction(nameof(GetProject), new { projectId = response.Id }, response);
    }

    [HttpPut("{projectId}")]
    public async Task<ActionResult<ProjectResponse>> PutProject(Guid projectId, UpdateProjectRequest request)
    {
        var response = await _projectService.UpdateProjectAsync(projectId, request);
        return Ok(response);
    }

    [HttpDelete("{projectId}")]
    public async Task<IActionResult> DeleteProject(Guid projectId)
    {
        await _projectService.DeleteProjectAsync(projectId);
        return NoContent();
    }
}