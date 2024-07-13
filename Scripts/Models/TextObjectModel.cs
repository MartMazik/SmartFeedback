using System.Text.Json.Serialization;
using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Models;

public class TextObjectModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; } = "";
    
    [JsonPropertyName("processed_content")]
    public string[] ProcessedContent { get; set; }
    
    [JsonPropertyName("project_id")]
    public string ProjectId { get; set; }
    
    [JsonPropertyName("user_rating_count")]
    public int UserRatingCount { get; set; }
    
    public TextObjectModel(TextObject textObject)
    {
        Id = textObject.Id.ToString() ?? string.Empty;
        Content = textObject.Content;
        ProcessedContent = textObject.ProcessedContent;
        ProjectId = textObject.ProjectId.ToString() ?? string.Empty;
        UserRatingCount = textObject.UserRatingCount;
    }

    public TextObjectModel()
    {
    }
}