namespace SmartFeedback.Scripts.Models;

public class UserRatingModel
{
    public int Id { get; set; }
    
    public int TextObjectId { get; set; }
    
    public string UserId { get; set; } = "";
    
    public bool IsLike { get; set; }


    public UserRatingModel(int id, int textObjectId, string userId, bool isLike)
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