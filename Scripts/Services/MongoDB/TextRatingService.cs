using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services.MongoDB;

public class TextRatingService : ITextRatingService
{
    public Task<bool> SetRating(UserRatingModel userRatingModel)
    {
        throw new NotImplementedException();
    }
}