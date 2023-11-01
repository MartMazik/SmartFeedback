using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Models;

public class ProjectModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    
    
    public ProjectModel(Project newProject)
    {
        Id = newProject.Id;
        Name = newProject.Name;
    }

    public ProjectModel(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public ProjectModel()
    {
    }
}