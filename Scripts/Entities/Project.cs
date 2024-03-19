using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Entities;

public class Project
{
    [BsonId]
    [BsonElement("_id")]
    public ObjectId Id { get; set; }

    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("title")]
    public string Title { get; set; }
    
    [BsonElement("language")]
    public string Language { get; set; }
    
    public Project(ProjectModel projectModel)
    {
        Id = new ObjectId(projectModel.Id);
        Title = projectModel.Name;
    }
    
    public Project()
    {
    }
}