using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartFeedback.Scripts.Entities;

public class ConnectTextsObjects
{
    [BsonId] [BsonElement("_id")] public ObjectId Id { get; set; }

    [BsonElement("is_deleted")] public bool IsDeleted { get; set; }

    [BsonElement("first_text_object_id")] public string FirstTextObjectId { get; set; }

    [BsonElement("second_text_object_id")] public string SecondTextObjectId { get; set; }

    [BsonElement("similarity")] public double Similarity { get; set; }

    public ConnectTextsObjects(string firstTextObjectId, string secondTextObjectId, double similarity)
    {
        FirstTextObjectId = firstTextObjectId;
        SecondTextObjectId = secondTextObjectId;
        Similarity = similarity;
    }

    public ConnectTextsObjects(ObjectId firstTextObject, ObjectId secondTextObject, double similarity)
    {
        FirstTextObjectId = firstTextObject.ToString();
        SecondTextObjectId = secondTextObject.ToString();
        Similarity = similarity;
    }

    public ConnectTextsObjects()
    {
    }
}