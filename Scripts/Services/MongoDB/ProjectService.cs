using MongoDB.Bson;
using MongoDB.Driver;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services.MongoDB;

public class ProjectService : IProjectService
{
    private readonly IMongoCollection<Project> _projects;

    public ProjectService(IMongoDatabase database)
    {
        _projects = database.GetCollection<Project>("projects");
    }

    public async Task<ProjectModel?> AddProject(string projectName)
    {
        var project = new Project { Name = projectName };
        await _projects.InsertOneAsync(project);
            
        return new ProjectModel(project);
    }

    public async Task<bool> DeleteProject(string projectId)
    {
        var objectId = new ObjectId(projectId);
        var project = await _projects.Find(x => x.Id == objectId).FirstOrDefaultAsync();
        if (project == null) return false;
        
        project.IsDeleted = true;
        
        await _projects.ReplaceOneAsync(x => x.Id == objectId, project);
        return true;
    }

    public async Task<bool> UnDeleteProject(string projectId)
    {
        var objectId = new ObjectId(projectId);
        var project = await _projects.Find(x => x.Id == objectId).FirstOrDefaultAsync();
        if (project == null) return false;
        
        project.IsDeleted = false;
        
        await _projects.ReplaceOneAsync(x => x.Id == objectId, project);
        return true;
    }

    public async Task<ProjectModel?> UpdateProject(ProjectModel projectModel)
    {
        if (projectModel.Name == string.Empty) return null;
        var objectId = new ObjectId(projectModel.Id);
        var project = await _projects.Find(x => x.Id == objectId).FirstOrDefaultAsync();
        if (project == null) return null;
        
        project.Name = projectModel.Name;
        
        await _projects.ReplaceOneAsync(x => x.Id == project.Id, project);
        return new ProjectModel(project);
    }

    public async Task<ProjectModel?> GetProject(string projectId)
    {
        var objectId = new ObjectId(projectId);
        var project = await _projects.Find(x => x.Id == objectId).FirstOrDefaultAsync();
        return project != null ? new ProjectModel(project) : null;
    }
    public async Task<List<ProjectModel>> GetFewProjects(int page = 1, int pageSize = 10)
    {
        var filter = Builders<Project>.Filter.Empty;
        var projects = await _projects.Find(filter).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

        return projects.ConvertAll(p => new ProjectModel(p));
    }
}