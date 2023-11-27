using MongoDB.Driver;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services.MongoDB;

public class ProjectService : IProjectService
{
    private readonly IMongoDatabase _database;

    public ProjectService(IMongoDatabase database)
    {
        _database = database;
    }
    
    public async Task<ProjectModel?> AddProject(string projectName)
    {
        var newProject = new Project { Name = projectName };
        await _database.InsertOneAsync(newProject);
        return new ProjectModel(newProject);
    }

    public async Task<bool> DeleteProject(int projectId)
    {
        var result = await _database.DeleteOneAsync(p => p.Id == projectId);
        return result.DeletedCount > 0;
    }

    public async Task<bool> UnDeleteProject(int projectId)
    {
        var result = await _database.UpdateOneAsync(
            p => p.Id == projectId && p.IsDeleted,
            Builders<Project>.Update.Set(p => p.IsDeleted, false));

        return result.ModifiedCount > 0;
    }

    public async Task<ProjectModel?> UpdateProject(ProjectModel projectModel)
    {
        var result = await _database.ReplaceOneAsync(
            p => p.Id == projectModel.Id,
            new Project { Id = projectModel.Id, Name = projectModel.Name });

        return result.ModifiedCount > 0 ? new ProjectModel(projectModel.Id, projectModel.Name) : null;
    }

    public async Task<ProjectModel?> GetProject(int projectId)
    {
        var project = await _database.Find(p => p.Id == projectId).FirstOrDefaultAsync();
        return project == null ? null : new ProjectModel(project.Id, project.Name);
    }

    public async Task<List<ProjectModel>> GetAllProject(int count = -1)
    {
        var projects = count == -1
            ? await _database.Find(_ => true).ToListAsync()
            : await _database.Find(_ => true).Limit(count).ToListAsync();

        return projects.Select(project => new ProjectModel(project.Id, project.Name)).ToList();
    }
}