using System.Text.Json.Serialization;
using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Models;

public class ProjectModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";
    
    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = "";
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("similarity_threshold")]
    public double SimilarityThreshold { get; set; } = 0.5;

    public ProjectModel(Project project)
    {
        Id = project.Id.ToString() ?? string.Empty;
        Title = project.Title;
        Language = project.Language;
        SimilarityThreshold = project.SimilarityThreshold;
        UserId = project.UserId.ToString() ?? string.Empty;
    }

    public ProjectModel()
    {
    }
}