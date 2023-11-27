using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartFeedback.Scripts.Entities
{
    public class ConnectTextsObjects
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public bool IsDeleted { get; set; }

        [BsonElement("FirstTextObject")]
        public TextObject FirstTextObject { get; set; }

        [BsonElement("SecondTextObject")]
        public TextObject SecondTextObject { get; set; }

        public double Coincidence { get; set; }

        public ConnectTextsObjects()
        {
        }
    }
}