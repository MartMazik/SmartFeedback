using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Models;

public class ProjectModel
{
    public string Id { get; set; } = "";
    public string Name { get; set; }
    
    public ProjectModel(Project project)
    {
        Id = project.Id.ToString() ?? string.Empty;
        Name = project.Name;
    }

    public ProjectModel(string id, string name)
    {
        Id = id;
        Name = name;
    }
    public ProjectModel(string name)
    {
        Name = name;
    }

    public ProjectModel() { }
}