using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Python.Runtime;
using SmartFeedback.Scripts.DataAnalysis;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Entities;

public class TextObject
{
    [BsonId]
    [BsonElement("_id")]
    public ObjectId Id { get; set; }

    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; }

    [BsonElement("content")]
    public string Content { get; set; } = "";

    [BsonElement("processed_content")]
    public string[] ProcessedContent { get; set; } = Array.Empty<string>();

    [BsonElement("project_id")]
    public string ProjectId { get; set; }

    [BsonElement("analog_count")]
    public int AnalogCount { get; set; }

    [BsonElement("user_rating_count")]
    public int UserRatingCount { get; set; }

    [BsonElement("rating_sum")]
    public int RatingSum { get; set; }

    public TextObject(TextObjectModel textObjectModel)
    {
        Id = new ObjectId(textObjectModel.Id);
        Content = textObjectModel.Content;
        ProjectId = textObjectModel.ProjectId;
        AnalogCount = textObjectModel.AnalogCount;
        UserRatingCount = textObjectModel.UserRatingCount;
        RatingSum = textObjectModel.RatingSum;
    }

    public TextObject(string content, string projectId)
    {
        Content = content;
        ProjectId = projectId;
    }

    public TextObject()
    {
    }
}