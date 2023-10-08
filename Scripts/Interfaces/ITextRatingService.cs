using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Interfaces;

public interface ITextRatingService
{
    public Task<bool> SetRating(UserRating userRating);
    public Task<bool> RemoveRating(UserRating userRating);
}