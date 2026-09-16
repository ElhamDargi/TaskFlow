using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlowApi.Data;
using TaskFlowApi.DTOs;
using TaskFlowApi.Models;

namespace TaskFlowApi.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly TaskFlowDbContext _context;

    public ProjectsController(TaskFlowDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
    {
        var projects = await _context.Projects.ToListAsync();
        return Ok(projects);
    }

    [HttpGet("{projectId}")]
    public async Task<ActionResult<Project>> GetProject(Guid projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
            return NotFound("Project not found");
        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<Project>> PostProject(CreateProjectRequest request)
    {
        var project = new Project
        {
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProject), new {projectId = project.Id}, project);
    }

    [HttpPut("{projectId}")]
    public async Task<ActionResult<Project>> PutProject(Guid projectId, UpdateProjectRequest request)
    {
        var projectEntity = await _context.Projects.FindAsync(projectId);
        if (projectEntity == null) return NotFound("Project not found");
        projectEntity.Name = request.Name;
        projectEntity.Description = request.Description;
        await _context.SaveChangesAsync();
        return Ok(projectEntity);
    }

    [HttpDelete("{projectId}")]
    public async Task<ActionResult> DeleteProject(Guid projectId)
    {
        var projectEntity = await _context.Projects.FindAsync(projectId);
        if (projectEntity == null) return NotFound("Project not found");
        _context.Projects.Remove(projectEntity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}