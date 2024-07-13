using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Interfaces;

public interface IProjectService
{
    public Task<ProjectModel?> CreateProject(ProjectModel projectModel);
    public Task<bool> DeleteProject(string projectId, string userId);
    public Task<bool> UnDeleteProject(string projectId, string userId);
    public Task<ProjectModel?> UpdateProject(ProjectModel projectModel);
    public Task<ProjectModel?> GetProject(string projectId);
    public Task<List<ProjectModel>> GetFewProjects(int page = 1, int pageSize = 10);
    public Task<List<ProjectModel>> SearchProjects(string searchString, int page = 1, int pageSize = 10);
    public Task<List<ProjectModel>> GetUserProjects(string userId, int page = 1, int pageSize = 10);
}