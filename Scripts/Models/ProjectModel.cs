using MongoDB.Bson;
using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Models;

public class ProjectModel
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    
    
    
    public ProjectModel(Project newProject)
    {
        Id = newProject.Id;
        Name = newProject.Name;
    }

    public ProjectModel(ObjectId id, string name)
    {
        Id = id;
        Name = name;
    }

    public ProjectModel()
    {
    }
}