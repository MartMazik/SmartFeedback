using Microsoft.EntityFrameworkCore;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services;

public class TextRatingService : ITextRatingService
{
    private readonly ApplicationContext _db;

    public TextRatingService(ApplicationContext db)
    {
        _db = db;
    }

    public async Task<bool> SetRating(UserRatingModel userRatingModel)
    {
        var textObject = await _db.TextObjects.FirstOrDefaultAsync(x => x.Id == userRatingModel.TextObjectId);
        if (textObject == null) return false;
        var rating = await _db.UserRatings.FirstOrDefaultAsync(x =>
            x.TextObject.Id == userRatingModel.TextObjectId && x.UserId == userRatingModel.UserId);
        if (rating == null)
        {
            rating = new UserRating(userRatingModel.UserId, userRatingModel.IsLike, textObject);
            await _db.UserRatings.AddAsync(rating);
        }
        else
        {
            rating.IsLike = userRatingModel.IsLike;
        }
        await _db.SaveChangesAsync();

        return true;
    }
}