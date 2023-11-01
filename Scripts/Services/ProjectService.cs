using Microsoft.EntityFrameworkCore;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services;

public class ProjectService : IProjectService
{
    private readonly ApplicationContext _db;

    public ProjectService(ApplicationContext db)
    {
        _db = db;
    }

    public async Task<ProjectModel?> AddProject(string projectName)
    {
        var newProject = new Project(projectName);
        _db.Projects.Add(newProject);
        await _db.SaveChangesAsync();
        return new ProjectModel(newProject);
    }

    public async Task<bool> DeleteProject(int projectId)
    {
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null) return false;
        project.IsDeleted = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnDeleteProject(int projectId)
    {
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null) return false;
        project.IsDeleted = false;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<ProjectModel?> UpdateProject(ProjectModel projectModel)
    {
        var oldProject = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectModel.Id);
        if (oldProject == null) return null;
        oldProject.Name = projectModel.Name;
        await _db.SaveChangesAsync();
        return new ProjectModel(oldProject);
    }

    public async Task<ProjectModel?> GetProject(int projectId)
    {
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        return project == null ? null : new ProjectModel(project);
    }

    public async Task<List<ProjectModel>> GetAllProject(int count = -1)
    {
        var projects = count == -1 ? await _db.Projects.ToListAsync() : await _db.Projects.Take(count).ToListAsync();
        return projects.Select(project => new ProjectModel(project)).ToList();
    }
}