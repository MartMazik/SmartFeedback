using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Entities;

public class UserRating
{
    [BsonId]
    [BsonElement("_id")]
    public ObjectId Id { get; set; }
    
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; }
    
    [BsonElement("user_id")]
    public string UserId { get; set; }
    
    [BsonElement("is_like")]
    public bool IsLike { get; set; }
    
    [BsonElement("text_object_id")]
    public string TextObjectId { get; set; }

    public UserRating(UserRatingModel userRatingModel)
    {
        Id = new ObjectId(userRatingModel.Id);
        UserId = userRatingModel.UserId;
        IsLike = userRatingModel.IsLike;
        TextObjectId = userRatingModel.TextObjectId;
    }
    
    public UserRating(string userId, bool isLike, string textObjectId)
    {
        UserId = userId;
        IsLike = isLike;
        TextObjectId = textObjectId;
    }
    
    public UserRating()
    {
    }
}