using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Entities;

public class Project
{
    [BsonId] [BsonElement("_id")] public ObjectId Id { get; set; }

    [BsonElement("is_deleted")] public bool IsDeleted { get; set; } = false;

    [BsonElement("user_id")] public ObjectId UserId { get; set; }

    [BsonElement("title")] public string Title { get; set; }

    [BsonElement("language")] public string Language { get; set; }

    [BsonElement("similarity_threshold")] public double SimilarityThreshold { get; set; } = 0.5;

    public Project(ProjectModel projectModel)
    {
        Id = ObjectId.GenerateNewId();
        Title = projectModel.Title;
        Language = projectModel.Language;
        SimilarityThreshold = projectModel.SimilarityThreshold;
        UserId = ObjectId.Parse(projectModel.UserId);
    }

    public Project() { }
}