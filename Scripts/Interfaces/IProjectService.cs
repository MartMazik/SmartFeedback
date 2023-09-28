using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Interfaces;

public interface IProjectService
{
    public Task<Project?> Add(Project project);
    public Task<bool> Delete(int id);
    public Task<bool> UnDelete(int id);
    public Task<Project?> Update(Project project);
    public Task<Project?> Get(int id);
    public Task<List<Project>> GetAll(int count = -1);
}