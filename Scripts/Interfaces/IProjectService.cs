using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Interfaces;

public interface IProjectService
{
    public Task<ProjectModel?> AddProject(string projectName);
    public Task<bool> DeleteProject(int projectId);
    public Task<bool> UnDeleteProject(int projectId);
    public Task<ProjectModel?> UpdateProject(ProjectModel projectModel);
    public Task<ProjectModel?> GetProject(int projectId);
    public Task<List<ProjectModel>> GetAllProject(int count = -1);
}