using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Python.Runtime;
using SmartFeedback.Scripts.DataAnalysis;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Entities;

public class TextObject
{
    [BsonId] public ObjectId Id { get; set; }

    public bool IsDeleted { get; set; }

    public string Content { get; set; } = "";

    public string[] ProcessedContend { get; set; } = Array.Empty<string>();

    public ObjectId ProjectId { get; set; }

    public int AnalogCount { get; set; }

    public int UserRatingCount { get; set; }

    public int RatingSum { get; set; }

    public TextObject(TextObjectModel textObjectModel)
    {
        Id = new ObjectId(textObjectModel.Id);
        Content = textObjectModel.Content;
        ProjectId = new ObjectId(textObjectModel.ProjectId);
        AnalogCount = textObjectModel.AnalogCount;
        UserRatingCount = textObjectModel.UserRatingCount;
        RatingSum = textObjectModel.RatingSum;
    }

    public TextObject(string content, string projectId, bool isPreprocessing = true)
    {
        Content = content;
        ProjectId = new ObjectId(projectId);
        
        if (isPreprocessing) ProcessedContend = Preprocessing.Preprocess(content).Split(" ");
    }

    public TextObject()
    {
    }
}