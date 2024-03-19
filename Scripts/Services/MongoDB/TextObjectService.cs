using MongoDB.Bson;
using MongoDB.Driver;
using SmartFeedback.Scripts.DataAnalysis;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services.MongoDB;

public class TextObjectService : ITextService
{
    private readonly IMongoCollection<TextObject> _texts;
    private readonly IMongoCollection<Project> _projects;
    private readonly ITextProcessService _textProcessService;

    public TextObjectService(IMongoDatabase database, ITextProcessService textProcessService)
    {
        _texts = database.GetCollection<TextObject>("text_object");
        _projects = database.GetCollection<Project>("project");
        _textProcessService = textProcessService;
    }

    public async Task<TextObjectModel?> AddOneText(TextObjectModel textObjectModel)
    {
        var textObject = new TextObject(textObjectModel.Content, textObjectModel.ProjectId);
        await Preprocessing.Preprocess(textObject);
        await _texts.InsertOneAsync(textObject);
        await _textProcessService.CompareTexts(textObjectModel.ProjectId);
        return new TextObjectModel(textObject);
    }

    public async Task<List<TextObjectModel>> AddMoreText(List<TextObjectModel> textObjectModels)
    {
        var textObjects = textObjectModels
            .ConvertAll(model => new TextObject(model.Content, model.ProjectId));
        
        await _texts.InsertManyAsync(textObjects);
        await _textProcessService.CompareTexts(textObjectModels[0].ProjectId);
        return textObjects.ConvertAll(textObject => new TextObjectModel(textObject));
    }

    public async Task<bool> DeleteText(string textId)
    {
        var objectId = new ObjectId(textId);
        var result = await _texts.DeleteOneAsync(x => x.Id == objectId);
        return result.DeletedCount > 0;
    }

    public async Task<bool> UnDeleteText(string textId)
    {
        var objectId = new ObjectId(textId);
        var update = Builders<TextObject>.Update.Set(x => x.IsDeleted, false);
        var result = await _texts.UpdateOneAsync(x => x.Id == objectId, update);
        return result.ModifiedCount > 0;
    }

    public async Task<TextObjectModel?> UpdateText(TextObjectModel textObjectModel)
    {
        var textObject = new TextObject(textObjectModel);

        var result = await _texts.ReplaceOneAsync(x => x.Id == textObject.Id, textObject);
        return result.ModifiedCount > 0 ? textObjectModel : null;
    }

    public async Task<TextObjectModel?> GetText(string textId)
    {
        var objectId = new ObjectId(textId);
        var textObject = await _texts.Find(x => x.Id == objectId).FirstOrDefaultAsync();
        return textObject != null ? new TextObjectModel(textObject) : null;
    }

    public async Task<List<TextObjectModel>> GetProjectsTexts(string projectId, int page = 1, int pageSize = 10)
    {
        var texts = await _texts.Find(x => x.ProjectId == projectId)
            .Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();
        return texts.ConvertAll(textObject => new TextObjectModel(textObject));
    }

    public async Task<bool> UploadTexts(IFormFile csvFile, string projectId)
    {
        var objectId = new ObjectId(projectId);
        var project = await _projects.Find(p => p.Id == objectId).FirstOrDefaultAsync();
        if (project == null) return false;

        var texts = new List<TextObject>();
        using var reader = new StreamReader(csvFile.OpenReadStream());
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (line == null) continue;
            var textObject = new TextObject(line, projectId);
            texts.Add(textObject);
        }
        await _texts.InsertManyAsync(texts);
        // TODO await _textProcessService.CompareTexts(projectId);
        return true;
    }
}