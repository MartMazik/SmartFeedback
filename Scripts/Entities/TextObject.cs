using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Entities;

public class TextObject
{
    [BsonId] [BsonElement("_id")] public ObjectId Id { get; set; }

    [BsonElement("is_deleted")] public bool IsDeleted { get; set; }

    [BsonElement("content")] public string Content { get; set; } = "";

    [BsonElement("processed_content")] public string[] ProcessedContent { get; set; } = Array.Empty<string>();

    [BsonElement("project_id")] public ObjectId ProjectId { get; set; }

    [BsonElement("user_rating_count")] public int UserRatingCount { get; set; }

    public TextObject(TextObjectModel textObjectModel)
    {
        Id = ObjectId.Parse(textObjectModel.Id);
        Content = textObjectModel.Content;
        ProcessedContent = textObjectModel.ProcessedContent;
        ProjectId = ObjectId.Parse(textObjectModel.ProjectId);
        UserRatingCount = textObjectModel.UserRatingCount;
    }
    
    public TextObject()
    {
    }
}