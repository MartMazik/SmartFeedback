using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Interfaces;

public interface IProjectService
{
    public Task<Project?> AddProject(Project project);
    public Task<bool> DeleteProject(int id);
    public Task<bool> UnDeleteProject(int id);
    public Task<Project?> UpdateProject(Project project);
    public Task<Project?> GetProject(int id);
    public Task<List<Project>> GetAllProject(int count = -1);
}