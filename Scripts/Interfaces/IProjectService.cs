using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Interfaces;

public interface IProjectService
{
    public Task<ProjectModel?> AddProject(string projectName);
    public Task<bool> DeleteProject(string projectId);
    public Task<bool> UnDeleteProject(string projectId);
    public Task<ProjectModel?> UpdateProject(ProjectModel projectModel);
    public Task<ProjectModel?> GetProject(string projectId);
    public Task<List<ProjectModel>> GetAllProject(int page = 1, int pageSize = 10);
}