using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Entities;

public class UserRating
{
    [BsonId] public ObjectId Id { get; set; }
    public bool IsDeleted { get; set; }
    public string UserId { get; set; }
    public bool IsLike { get; set; }

    public ObjectId TextObjectId { get; set; }

    public UserRating(UserRatingModel userRatingModel)
    {
        Id = new ObjectId(userRatingModel.Id);
        UserId = userRatingModel.UserId;
        IsLike = userRatingModel.IsLike;
        TextObjectId = new ObjectId(userRatingModel.TextObjectId);
    }
    
    public UserRating(string userId, bool isLike, string textObjectId)
    {
        UserId = userId;
        IsLike = isLike;
        TextObjectId = new ObjectId(textObjectId);
    }
    
    public UserRating()
    {
    }
}