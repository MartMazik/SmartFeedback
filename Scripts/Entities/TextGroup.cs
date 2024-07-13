using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Entities;

public class TextGroup
{
    [BsonId] [BsonElement("_id")] public ObjectId Id { get; set; }

    [BsonElement("is_deleted")] public bool IsDeleted { get; set; } = false;

    [BsonElement("analog_count")] public int AnalogCount { get; set; } = 0;
    
    [BsonElement("core_text")] public string CoreText { get; set; }

    [BsonElement("project_id")] public ObjectId ProjectId { get; set; }

    [BsonElement("center_text_id")] public ObjectId CenterTextId { get; set; }

    [BsonElement("text_ids")] public ObjectId[] TextIds { get; set; }

    public TextGroup(TextGroupModel textGroupModel)
    {
        Id = ObjectId.Parse(textGroupModel.Id);
        AnalogCount = textGroupModel.AnalogCount;
        CoreText = textGroupModel.CoreText;
        ProjectId = ObjectId.Parse(textGroupModel.ProjectId);
        CenterTextId = ObjectId.Parse(textGroupModel.CentralText.Id);
        TextIds = textGroupModel.AnalogTexts.Select(analogText => ObjectId.Parse(analogText.Id)).ToArray();
    }

    public TextGroup() { }
}