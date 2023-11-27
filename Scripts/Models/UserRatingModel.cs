using MongoDB.Bson;

namespace SmartFeedback.Scripts.Models;

public class UserRatingModel
{
    public ObjectId Id { get; set; }
    
    public ObjectId TextObjectId { get; set; }
    
    public string UserId { get; set; } = "";
    
    public bool IsLike { get; set; }


    public UserRatingModel(ObjectId id, ObjectId textObjectId, string userId, bool isLike)
    {
        Id = id;
        TextObjectId = textObjectId;
        UserId = userId;
        IsLike = isLike;
    }

    public UserRatingModel()
    {
    }
}