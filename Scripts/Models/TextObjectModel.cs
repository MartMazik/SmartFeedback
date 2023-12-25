using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Models;

public class TextObjectModel
{
    public string Id { get; set; }

    public string Content { get; set; } = "";
    
    public string ProjectId { get; set; }
    
    public int AnalogCount { get; set; }
    
    public int UserRatingCount { get; set; }
    
    public int RatingSum { get; set; }
    
    
    
    public TextObjectModel(TextObject textObject)
    {
        Id = textObject.Id.ToString() ?? string.Empty;
        Content = textObject.Content;
        ProjectId = textObject.ProjectId.ToString() ?? string.Empty;
        AnalogCount = textObject.AnalogCount;
        UserRatingCount = textObject.UserRatingCount;
        RatingSum = textObject.RatingSum;
    }

    public TextObjectModel(string id, string content, string projectId, int analogCount, int userRatingCount, int ratingSum)
    {
        Id = id;
        Content = content;
        ProjectId = projectId;
        AnalogCount = analogCount;
        UserRatingCount = userRatingCount;
        RatingSum = ratingSum;
    }

    public TextObjectModel() { }
}