using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartFeedback.Scripts.Entities;

public class ConnectTextsObjects
{
    [BsonId] public ObjectId Id { get; set; }

    public bool IsDeleted { get; set; }

    public ObjectId FirstTextObject { get; set; }

    public ObjectId SecondTextObject { get; set; }

    public double Coincidence { get; set; }

    public ConnectTextsObjects(ObjectId firstTextObject, ObjectId secondTextObject, double coincidence)
    {
        FirstTextObject = firstTextObject;
        SecondTextObject = secondTextObject;
        Coincidence = coincidence;
    }
    public ConnectTextsObjects()
    {
    }
}