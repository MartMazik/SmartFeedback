using MongoDB.Bson;
using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Models;

public class TextObjectModel
{
    public ObjectId Id { get; set; }

    public string Content { get; set; } = "";
    
    public ObjectId ProjectId { get; set; }
    
    public int AnalogCount { get; set; }
    
    public int UserRatingCount { get; set; }
    
    public int RatingSum { get; set; }
    
    
    
    public TextObjectModel(TextObject textObject)
    {
        Id = textObject.Id;
        Content = textObject.Content;
        ProjectId = textObject.Project.Id;
        AnalogCount = textObject.AnalogCount;
        UserRatingCount = textObject.UserRatingCount;
        RatingSum = textObject.RatingSum;
    }

    public TextObjectModel(ObjectId id, string content, ObjectId projectId, int analogCount, int userRatingCount, int ratingSum)
    {
        Id = id;
        Content = content;
        ProjectId = projectId;
        AnalogCount = analogCount;
        UserRatingCount = userRatingCount;
        RatingSum = ratingSum;
    }

    public TextObjectModel()
    {
    }
}