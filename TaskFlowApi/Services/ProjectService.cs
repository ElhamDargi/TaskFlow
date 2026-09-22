namespace TaskFlowApi.Services;

using Microsoft.EntityFrameworkCore;
using TaskFlowApi.Data;
using TaskFlowApi.DTOs;
using TaskFlowApi.Exceptions;
using TaskFlowApi.Mappers;
using Project = TaskFlowApi.Models.Project;

public class ProjectService
{
    private readonly TaskFlowDbContext _context;

    public ProjectService(TaskFlowDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectResponse>> GetAllProjectsAsync()
    {
        var projects = await _context.Projects.ToListAsync();
        return projects.Select(ProjectMapper.ToResponse);
    }

    public async Task<ProjectResponse> GetProjectByIdAsync(Guid projectId)
    {
        var project = await _context.Projects.FindAsync(projectId)
                      ?? throw new NotFoundException("Project not found");

        return ProjectMapper.ToResponse(project);
    }

    public async Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request)
    {
        var project = new Project
        {
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return ProjectMapper.ToResponse(project);
    }

    public async Task<ProjectResponse> UpdateProjectAsync(Guid projectId, UpdateProjectRequest request)
    {
        var projectEntity = await _context.Projects.FindAsync(projectId)
                            ?? throw new NotFoundException("Project not found");

        projectEntity.Name = request.Name;
        projectEntity.Description = request.Description;

        await _context.SaveChangesAsync();

        return ProjectMapper.ToResponse(projectEntity);
    }

    public async Task DeleteProjectAsync(Guid projectId)
    {
        var rowsAffected = await _context.Projects
            .Where(p => p.Id == projectId)
            .ExecuteDeleteAsync();

        if (rowsAffected == 0)
        {
            throw new NotFoundException("Project not found");
        }
    }
}