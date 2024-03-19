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
        _texts = database.GetCollection<TextObject>("text_object");
        _connectTextsObjects = database.GetCollection<ConnectTextsObjects>("connect_texts_object");
    }
    
    public async Task<bool> UpdateTextPreprocessing(string projectId)
    {
        Console.WriteLine("Start UpdateTextPreprocessing");
        var texts = await _texts.Find(x => x.ProjectId == projectId).ToListAsync();
        foreach (var text in texts)
        {
            // text.ProcessedContend = Preprocessing.Preprocess(text.Content).Result.Split(" ");
            await _texts.ReplaceOneAsync(x => x.Id == text.Id, text);
        }
        return texts.Count > 0;
    }
    

    public async Task<bool> CompareTexts(string projectId)
    {
        Console.WriteLine("Start CompareTexts");
        var texts = await _texts.Find(x => x.ProjectId == projectId).ToListAsync();
        var connectTextsObjectsList = TfidfAlgorithm.Start(texts);
        await _connectTextsObjects.InsertManyAsync(connectTextsObjectsList);
        return texts.Count > 1;
    }
}