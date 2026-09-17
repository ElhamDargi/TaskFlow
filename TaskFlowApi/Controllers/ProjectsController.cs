using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlowApi.Data;
using TaskFlowApi.DTOs;
using TaskFlowApi.Mappers;
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
        var response = projects.Select(ProjectMapper.ToResponse).ToList();
        return Ok(response);
    }

    [HttpGet("{projectId}")]
    public async Task<ActionResult<Project>> GetProject(Guid projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
            return NotFound("Project not found");
        var response = ProjectMapper.ToResponse(project);
        return Ok(response);
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
        var response = ProjectMapper.ToResponse(project);
        return CreatedAtAction(nameof(GetProject), new {projectId = response.Id}, response);
    }

    [HttpPut("{projectId}")]
    public async Task<ActionResult<Project>> PutProject(Guid projectId, UpdateProjectRequest request)
    {
        var projectEntity = await _context.Projects.FindAsync(projectId);
        if (projectEntity == null) return NotFound("Project not found");
        projectEntity.Name = request.Name;
        projectEntity.Description = request.Description;
        await _context.SaveChangesAsync();
        var response = ProjectMapper.ToResponse(projectEntity);
        return Ok(response);
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