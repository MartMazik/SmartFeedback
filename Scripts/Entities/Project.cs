using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Entities;

public class Project
{
    [BsonId] public ObjectId Id { get; set; }

    public bool IsDeleted { get; set; }

    public string Name { get; set; }

    public Project(ProjectModel projectModel)
    {
        Id = new ObjectId(projectModel.Id);
        Name = projectModel.Name;
    }
    
    public Project()
    {
    }
}