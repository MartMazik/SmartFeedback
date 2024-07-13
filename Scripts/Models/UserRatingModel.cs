using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Models;

public class UserRatingModel
{
    public string Id { get; set; }
    
    public string TextObjectId { get; set; }
    
    public string UserId { get; set; }


    public UserRatingModel(UserRating userRating)
    {
        Id = userRating.Id.ToString() ?? string.Empty;
        TextObjectId = userRating.TextObjectId.ToString() ?? string.Empty;
        UserId = userRating.UserId.ToString() ?? string.Empty;
    }

    public UserRatingModel() { }
}