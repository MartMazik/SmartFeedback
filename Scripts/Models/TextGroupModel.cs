using System.Text.Json.Serialization;
using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Models;

public class TextGroupModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("project_id")]
    public string ProjectId { get; set; }
    
    [JsonPropertyName("analog_count")]
    public int AnalogCount { get; set; }
    
    [JsonPropertyName("core_text")]
    public string CoreText { get; set; }
    
    [JsonPropertyName("center_text")]
    public TextObjectModel CentralText { get; set; }
    
    [JsonPropertyName("analog_texts")]
    public List<TextObjectModel> AnalogTexts { get; set; }

    public TextGroupModel(TextGroup textGroup, TextObject centralText, List<TextObject> analogTexts)
    {
        Id = textGroup.Id.ToString() ?? string.Empty;
        ProjectId = textGroup.ProjectId.ToString() ?? string.Empty;
        AnalogCount = textGroup.AnalogCount;
        CoreText = textGroup.CoreText;
        CentralText = new TextObjectModel(centralText);
        AnalogTexts = analogTexts.Select(textObject => new TextObjectModel(textObject)).ToList();
    }
    
    public TextGroupModel(TextGroup textGroup, TextObject centralText)
    {
        Id = textGroup.Id.ToString() ?? string.Empty;
        ProjectId = textGroup.ProjectId.ToString() ?? string.Empty;
        AnalogCount = textGroup.AnalogCount;
        CoreText = textGroup.CoreText;
        CentralText = new TextObjectModel(centralText);
        AnalogTexts = [];
    }

    public TextGroupModel()
    {
    }
}