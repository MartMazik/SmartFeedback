using MongoDB.Bson;
using MongoDB.Driver;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services.MongoDB;

public class ProjectService : IProjectService
{
    private readonly IMongoCollection<Project> _projects;
    private readonly IProcessingModuleService _processingModuleService;

    public ProjectService(IMongoDatabase database, IProcessingModuleService processingModuleService)
    {
        _projects = database.GetCollection<Project>("project");
        _processingModuleService = processingModuleService;
    }

    public async Task<ProjectModel?> CreateProject(ProjectModel projectModel)
    {
        // check - title is not empty, language is not empty, similarity threshold is between 0 and 1 and userId is not empty
        if (projectModel.Title == string.Empty || projectModel.Language == string.Empty ||
            projectModel.SimilarityThreshold < 0 || projectModel.SimilarityThreshold > 1 ||
            projectModel.UserId == string.Empty) return null;

        projectModel.Language = projectModel.Language.ToLower() switch
        {
            "ru" => "russian",
            "rus" => "russian",
            "ru-ru" => "russian",
            "russian" => "russian",
            "русский" => "russian",
            "русский язык" => "russian",
            "ру" => "russian",
            _ => "english"
        };

        var project = new Project
        {
            Title = projectModel.Title,
            Language = projectModel.Language,
            SimilarityThreshold = projectModel.SimilarityThreshold,
            UserId = new ObjectId(projectModel.UserId)
        };

        await _projects.InsertOneAsync(project);
        return new ProjectModel(project);
    }

    public async Task<bool> DeleteProject(string projectId, string userId)
    {
        var projectIdObject = new ObjectId(projectId);
        var userIdObject = new ObjectId(userId);
        // project and user exist
        var project = await _projects.Find(x => x.Id == projectIdObject && x.UserId == userIdObject)
            .FirstOrDefaultAsync();
        if (project == null) return false;

        project.IsDeleted = true;

        await _projects.ReplaceOneAsync(x => x.Id == projectIdObject, project);
        return true;
    }

    public async Task<bool> UnDeleteProject(string projectId, string userId)
    {
        var projectIdObject = new ObjectId(projectId);
        var userIdObject = new ObjectId(userId);
        // project and user exist
        var project = await _projects.Find(x => x.Id == projectIdObject && x.UserId == userIdObject)
            .FirstOrDefaultAsync();
        if (project == null) return false;

        project.IsDeleted = false;

        await _projects.ReplaceOneAsync(x => x.Id == projectIdObject, project);
        return true;
    }

    public async Task<ProjectModel?> UpdateProject(ProjectModel projectModel)
    {
        // check - title is not empty, language is not empty, similarity threshold is between 0 and 1 and userId is not empty
        if (projectModel.Title == string.Empty || projectModel.Language == string.Empty ||
            projectModel.SimilarityThreshold < 0 || projectModel.SimilarityThreshold > 1 ||
            projectModel.UserId == string.Empty) return null;

        var projectId = new ObjectId(projectModel.Id);
        var userId = new ObjectId(projectModel.UserId);
        // select by id and user id
        var project = await _projects.Find(x => x.Id == projectId && x.UserId == userId).FirstOrDefaultAsync();
        if (project == null) return null;

        project.Title = projectModel.Title;
        project.Language = projectModel.Language;
        project.SimilarityThreshold = projectModel.SimilarityThreshold;

        await _projects.ReplaceOneAsync(x => x.Id == projectId, project);

        var isPreprocessed = await _processingModuleService.ReComparisonInProject(projectModel.Id);
        return !isPreprocessed ? null : new ProjectModel(project);
    }

    public async Task<ProjectModel?> GetProject(string projectId)
    {
        var projectIdObject = new ObjectId(projectId);
        var project = await _projects.Find(x => x.Id == projectIdObject).FirstOrDefaultAsync();
        return project == null ? null : new ProjectModel(project);
    }

    public async Task<List<ProjectModel>> GetFewProjects(int page = 1, int pageSize = 10)
    {
        var filter = Builders<Project>.Filter.Eq(p => p.IsDeleted, false);
        var projects = await _projects.Find(filter).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

        return projects.ConvertAll(p => new ProjectModel(p));
    }

    public async Task<List<ProjectModel>> SearchProjects(string searchString, int page = 1, int pageSize = 10)
    {
        var projects = await _processingModuleService.SearchProject(searchString);
        projects = projects.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return projects.ConvertAll(p => new ProjectModel(p));
    }

    public async Task<List<ProjectModel>> GetUserProjects(string userId, int page = 1, int pageSize = 10)
    {
        var objectId = new ObjectId(userId);
        var filter = Builders<Project>.Filter.Eq(p => p.UserId, objectId) &
                     Builders<Project>.Filter.Eq(p => p.IsDeleted, false);
        var projects = await _projects.Find(filter).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

        return projects.ConvertAll(p => new ProjectModel(p));
    }

    public async Task<List<ProjectModel>> GetDeletedProjects(int page = 1, int pageSize = 10)
    {
        var filter = Builders<Project>.Filter.Eq(p => p.IsDeleted, true);
        var projects = await _projects.Find(filter).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

        return projects.ConvertAll(p => new ProjectModel(p));
    }
}