using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Models;

public class UserRatingModel
{
    public string Id { get; set; }
    
    public string TextObjectId { get; set; }
    
    public string UserId { get; set; } = "";
    
    public bool IsLike { get; set; }


    public UserRatingModel(UserRating userRating)
    {
        Id = userRating.Id.ToString() ?? string.Empty;
        TextObjectId = userRating.TextObjectId.ToString() ?? string.Empty;
        UserId = userRating.UserId;
        IsLike = userRating.IsLike;
    }
    
    public UserRatingModel(string id, string textObjectId, string userId, bool isLike)
    {
        Id = id;
        TextObjectId = textObjectId;
        UserId = userId;
        IsLike = isLike;
    }

    public UserRatingModel() { }
}