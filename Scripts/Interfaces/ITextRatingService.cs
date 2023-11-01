using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Interfaces;

public interface ITextRatingService
{
    public Task<bool> SetRating(UserRatingModel userRatingModel);
}