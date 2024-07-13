using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Entities;

public class UserRating
{
    [BsonId] [BsonElement("_id")] public ObjectId Id { get; set; }

    [BsonElement("is_deleted")] public bool IsDeleted { get; set; }

    [BsonElement("user_id")] public ObjectId UserId { get; set; }

    [BsonElement("text_object_id")] public ObjectId TextObjectId { get; set; }

    public UserRating(UserRatingModel userRatingModel)
    {
        Id = ObjectId.Parse(userRatingModel.Id);
        UserId = ObjectId.Parse(userRatingModel.UserId);
        TextObjectId = ObjectId.Parse(userRatingModel.TextObjectId);
    }
    
    public UserRating()
    {
    }
}