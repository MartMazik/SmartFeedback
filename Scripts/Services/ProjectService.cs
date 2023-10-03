using Microsoft.EntityFrameworkCore;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;

namespace SmartFeedback.Scripts.Services;

public class ProjectService : IProjectService
{
    private readonly ApplicationContext _db;

    public ProjectService(ApplicationContext db)
    {
        _db = db;
    }

    public async Task<Project?> AddProject(Project project)
    {
        _db.Projects.Add(project);
        await _db.SaveChangesAsync();
        return project;
    }

    public async Task<bool> DeleteProject(int id)
    {
        var project = _db.Projects.FirstOrDefault(p => p.Id == id);
        if (project == null) return false;
        project.IsDeleted = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnDeleteProject(int id)
    {
        var project = _db.Projects.FirstOrDefault(p => p.Id == id);
        if (project == null) return false;
        project.IsDeleted = false;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<Project?> UpdateProject(Project project)
    {
        var oldProject = _db.Projects.FirstOrDefault(p => p.Id == project.Id);
        if (oldProject == null) return null;
        oldProject.Name = project.Name;
        await _db.SaveChangesAsync();
        return oldProject;
    }

    public async Task<Project?> GetProject(int id)
    {
        return await _db.Projects.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Project>> GetAllProject(int count = -1)
    {
        return count == -1 ? await _db.Projects.ToListAsync() : await _db.Projects.Take(count).ToListAsync();
    }
}