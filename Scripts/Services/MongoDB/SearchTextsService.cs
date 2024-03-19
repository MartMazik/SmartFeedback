using MongoDB.Bson;
using MongoDB.Driver;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services.MongoDB;

public class SearchTextsService : ISearchTextsService
{
    private readonly IMongoCollection<TextObject> _texts;

    public SearchTextsService(IMongoDatabase database)
    {
        _texts = database.GetCollection<TextObject>("text_object");
    }

    public async Task<List<TextObjectModel>> GetSimilarTexts(TextObjectModel textObjectModel, int page = 1,
        int pageSize = 10)
    {
        // TODO Переделать на поиск по векторам или другим алгоритмом

        var textObject = new TextObject(textObjectModel);
        // Получаем тексты, которые не были удалены и не являются текущим текстом, но лежат в том же проекте
        var filter = Builders<TextObject>.Filter.Eq(x => x.IsDeleted, false) &
                     Builders<TextObject>.Filter.Ne(x => x.Id, textObject.Id) &
                     Builders<TextObject>.Filter.Eq(x => x.ProjectId, textObject.ProjectId);
        var texts = await _texts.Find(filter).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

        return texts.ConvertAll(text => new TextObjectModel(text));
    }

    public async Task<List<TextObjectModel>> GetSimilarTextsById(string textId, int page = 1, int pageSize = 10)
    {
        // TODO Переделать на поиск по векторам или другим алгоритмом

        var objectId = new ObjectId(textId);
        var textObject = await _texts.Find(x => x.Id == objectId).FirstOrDefaultAsync();

        // Получаем тексты, которые не были удалены и не являются текущим текстом, но лежат в том же проекте
        var filter = Builders<TextObject>.Filter.Eq(x => x.IsDeleted, false) &
                     Builders<TextObject>.Filter.Ne(x => x.Id, textObject.Id) &
                     Builders<TextObject>.Filter.Eq(x => x.ProjectId, textObject.ProjectId);
        var texts = await _texts.Find(filter).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

        return texts.ConvertAll(text => new TextObjectModel(text));
    }
}