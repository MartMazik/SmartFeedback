using MongoDB.Bson;
using MongoDB.Driver;
using SmartFeedback.Scripts.DataAnalysis;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;

namespace SmartFeedback.Scripts.Services.Process;

public class TextProcessService : ITextProcessService
{
    private readonly IMongoCollection<TextObject> _texts;
    private readonly IMongoCollection<ConnectTextsObjects> _connectTextsObjects;

    public TextProcessService(IMongoDatabase database)
    {
        _texts = database.GetCollection<TextObject>("texts");
        _connectTextsObjects = database.GetCollection<ConnectTextsObjects>("connectTextsObjects");
    }
    
    public async Task<bool> UpdateTextPreprocessing(string projectId)
    {
        Console.WriteLine("Start UpdateTextPreprocessing");
        var objectId = new ObjectId(projectId);
        var texts = await _texts.Find(x => x.ProjectId == objectId).ToListAsync();
        foreach (var text in texts)
        {
            text.ProcessedContend = Preprocessing.Preprocess(text.Content).Split(" ");
            await _texts.ReplaceOneAsync(x => x.Id == text.Id, text);
        }
        return texts.Count > 0;
    }
    

    public async Task<bool> CompareTexts(string projectId)
    {
        Console.WriteLine("Start CompareTexts");
        var objectId = new ObjectId(projectId);
        var texts = await _texts.Find(x => x.ProjectId == objectId).ToListAsync();
        var connectTextsObjectsList = TfidfAlgorithm.Start(texts);
        await _connectTextsObjects.InsertManyAsync(connectTextsObjectsList);
        return texts.Count > 1;
    }
}