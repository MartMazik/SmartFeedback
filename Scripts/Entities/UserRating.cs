using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartFeedback.Scripts.Entities
{
    public class UserRating
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public bool IsDeleted { get; set; }

        public string UserId { get; set; }

        public bool IsLike { get; set; }

        [BsonElement("TextObject")]
        public TextObject TextObject { get; set; }

        public UserRating()
        {
        }

        public UserRating(string userId, bool isLike, TextObject textObject)
        {
            UserId = userId;
            IsLike = isLike;
            TextObject = textObject;
        }
    }
}