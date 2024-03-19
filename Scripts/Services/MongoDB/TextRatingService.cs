using MongoDB.Driver;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services.MongoDB;

public class TextRatingService : ITextRatingService
{
    private readonly IMongoCollection<UserRating> _textRatings;

    public TextRatingService(IMongoDatabase database)
    {
        _textRatings = database.GetCollection<UserRating>("text_rating");
    }

    public async Task<bool> SetRating(UserRatingModel userRatingModel)
    {
        var userRating = new UserRating(userRatingModel.UserId, userRatingModel.IsLike, userRatingModel.TextObjectId);

        // Проверка, существует ли рейтинг пользователя для данного текста
        var existingRating = await _textRatings
            .Find(x => x.TextObjectId == userRating.TextObjectId && x.UserId == userRating.UserId)
            .FirstOrDefaultAsync();

        if (existingRating == null)
        {
            // Если рейтинга нет, добавляем новый
            await _textRatings.InsertOneAsync(userRating);
        }
        else
        {
            // Если рейтинг уже существует, обновляем его
            var update = Builders<UserRating>.Update.Set(x => x.IsLike, userRating.IsLike);
            await _textRatings.UpdateOneAsync(x => x.Id == existingRating.Id, update);
        }

        return true;
    }
}