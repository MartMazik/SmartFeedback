using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services.MongoDB;

public class ProjectService : IProjectService
{
    public Task<ProjectModel?> AddProject(string projectName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteProject(int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UnDeleteProject(int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectModel?> UpdateProject(ProjectModel projectModel)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectModel?> GetProject(int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ProjectModel>> GetAllProject(int count = -1)
    {
        throw new NotImplementedException();
    }
}