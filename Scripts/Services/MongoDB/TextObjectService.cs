using MongoDB.Bson;
using MongoDB.Driver;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services.MongoDB;

public class TextObjectService : ITextObjectService
{
    private readonly IMongoCollection<TextObject> _texts;
    private readonly IMongoCollection<Project> _projects;
    private readonly IMongoCollection<TextGroup> _textGroups;
    private readonly IMongoCollection<UserRating> _userRatings;
    private readonly IProcessingModuleService _processingModuleService;


    public TextObjectService(IMongoDatabase database, IProcessingModuleService processingModuleService)
    {
        _texts = database.GetCollection<TextObject>("text_object");
        _projects = database.GetCollection<Project>("project");
        _textGroups = database.GetCollection<TextGroup>("text_group");
        _userRatings = database.GetCollection<UserRating>("user_rating");
        _processingModuleService = processingModuleService;
    }

    public async Task<TextObjectModel?> CreateOneText(TextObjectModel textObjectModel, string userId)
    {
        if (textObjectModel.Content == string.Empty) return null;
        var projectIdObject = new ObjectId(textObjectModel.ProjectId);
        var userIdObject = new ObjectId(userId);
        var project = await _projects.Find(x => x.Id == projectIdObject && x.UserId == userIdObject)
            .FirstOrDefaultAsync();
        if (project == null) return null;

        var text = new TextObject
        {
            Content = textObjectModel.Content,
            ProjectId = projectIdObject,
            UserRatingCount = 0
        };

        text = await _processingModuleService.PreprocessingOne(text);
        if (text == null) return null;
        await _texts.InsertOneAsync(text);

        await _processingModuleService.ComparisonOne(text);

        return new TextObjectModel(text);
    }

    public async Task<List<TextObjectModel>> CreateFewText(List<TextObjectModel> textObjectModels, string userId)
    {
        if (textObjectModels.Count == 0) return [];
        var projectIdObject = new ObjectId(textObjectModels[0].ProjectId);
        var userIdObject = new ObjectId(userId);
        var project = await _projects.Find(x => x.Id == projectIdObject && x.UserId == userIdObject)
            .FirstOrDefaultAsync();
        if (project == null) return [];

        var texts = textObjectModels.Select(textObjectModel => new TextObject
        {
            Content = textObjectModel.Content,
            ProjectId = projectIdObject,
            UserRatingCount = 0
        }).ToList();

        texts = await _processingModuleService.PreprocessingFew(texts);
        if (texts.Count == 0) return [];

        await _texts.InsertManyAsync(texts);

        _ = _processingModuleService.ComparisonFew(texts);

        return texts.Select(text => new TextObjectModel(text)).ToList();
    }

    public async Task<bool> UploadTexts(IFormFile csvFile, string projectId, string userId)
    {
        var projectIdObject = new ObjectId(projectId);
        var userIdObject = new ObjectId(userId);
        var project = await _projects.Find(x => x.Id == projectIdObject && x.UserId == userIdObject)
            .FirstOrDefaultAsync();
        if (project == null) return false;

        var texts = new List<TextObject>();
        using (var reader = new StreamReader(csvFile.OpenReadStream()))
        {
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (line == null) continue;
                texts.Add(new TextObject
                {
                    Content = line,
                    ProjectId = projectIdObject,
                    UserRatingCount = 0
                });
            }
        }

        texts = await _processingModuleService.PreprocessingFew(texts);
        if (texts.Count == 0) return false;

        await _texts.InsertManyAsync(texts);

        _ = _processingModuleService.ComparisonFew(texts);

        return true;
    }

    public async Task<bool> DeleteText(string textId, string userId)
    {
        var textIdObject = new ObjectId(textId);
        var userIdObject = new ObjectId(userId);
        var text = await _texts.Find(x => x.Id == textIdObject).FirstOrDefaultAsync();
        if (text == null) return false;
        var project = await _projects.Find(x => x.Id == text.ProjectId && x.UserId == userIdObject)
            .FirstOrDefaultAsync();
        if (project == null) return false;

        text.IsDeleted = true;
        await _texts.ReplaceOneAsync(x => x.Id == textIdObject, text);
        
        _ = _processingModuleService.ReComparisonInProject(project.Id.ToString());
        
        return true;
    }

    public async Task<bool> UnDeleteText(string textId, string userId)
    {
        var textIdObject = new ObjectId(textId);
        var userIdObject = new ObjectId(userId);
        var text = await _texts.Find(x => x.Id == textIdObject).FirstOrDefaultAsync();
        if (text == null) return false;
        var project = await _projects.Find(x => x.Id == text.ProjectId && x.UserId == userIdObject)
            .FirstOrDefaultAsync();
        if (project == null) return false;

        text.IsDeleted = false;
        await _texts.ReplaceOneAsync(x => x.Id == textIdObject, text);
        
        _ = _processingModuleService.ReComparisonInProject(project.Id.ToString());
        
        return true;
    }

    public async Task<TextObjectModel?> UpdateText(TextObjectModel textObjectModel, string userId)
    {
        var textIdObject = new ObjectId(textObjectModel.Id);
        var userIdObject = new ObjectId(userId);
        var text = await _texts.Find(x => x.Id == textIdObject).FirstOrDefaultAsync();
        if (text == null) return null;
        var text1 = text;
        var project = await _projects.Find(x => x.Id == text1.ProjectId && x.UserId == userIdObject)
            .FirstOrDefaultAsync();
        if (project == null) return null;

        text.Content = textObjectModel.Content;

        text = await _processingModuleService.PreprocessingOne(text);
        if (text == null) return null;
        await _texts.ReplaceOneAsync(x => x.Id == textIdObject, text);
        
        _ = _processingModuleService.ReComparisonInProject(project.Id.ToString());
        
        return new TextObjectModel(text);
    }

    public async Task<TextObjectModel?> GetText(string textId)
    {
        var textIdObject = new ObjectId(textId);
        var text = await _texts.Find(x => x.Id == textIdObject).FirstOrDefaultAsync();
        return text == null ? null : new TextObjectModel(text);
    }

    public async Task<List<TextGroupModel>> GetTextsByProject(string projectId, int page = 1, int pageSize = 20)
    {
        var projectIdObject = new ObjectId(projectId);
        var textGroups = await _textGroups
            .Find(x => x.ProjectId == projectIdObject && !x.IsDeleted)
            .SortByDescending(x => x.AnalogCount)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        var textGroupModels = new List<TextGroupModel>();

        foreach (var textGroup in textGroups)
        {
            var centerText = await _texts
                .Find(x => x.Id == textGroup.CenterTextId && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if (centerText == null) continue;

            var analogTexts = await _texts
                .Find(x => textGroup.TextIds.Contains(x.Id) && !x.IsDeleted)
                .ToListAsync();

            textGroupModels.Add(new TextGroupModel(textGroup, centerText, analogTexts));
        }

        return textGroupModels;
    }

    public async Task<List<TextGroupModel>?> SearchTexts(string projectId, string searchString, int page = 1,
        int pageSize = 20)
    {
        var processedTextGroupModels = await _processingModuleService.SearchText(searchString, projectId);

        var filteredTextGroupModels = processedTextGroupModels
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return filteredTextGroupModels;
    }

    public async Task<TextGroupModel?> GetTextsFromGroup(string groupId, int page = 1, int pageSize = 20)
    {
        var groupIdObject = new ObjectId(groupId);
        var textGroup = await _textGroups
            .Find(x => x.Id == groupIdObject && !x.IsDeleted)
            .FirstOrDefaultAsync();
        if (textGroup == null) return null;

        var centerText = await _texts
            .Find(x => x.Id == textGroup.CenterTextId && !x.IsDeleted)
            .FirstOrDefaultAsync();
        if (centerText == null) return null;

        var analogTexts = await _texts
            .Find(x => textGroup.TextIds.Contains(x.Id) && !x.IsDeleted)
            .SortByDescending(x => x.UserRatingCount)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return analogTexts.Count == 0
            ? new TextGroupModel(textGroup, centerText)
            : new TextGroupModel(textGroup, centerText, analogTexts);
    }

    public async Task<List<TextObjectModel>> GetDeletedTexts(int page = 1, int pageSize = 20)
    {
        var texts = await _texts
            .Find(x => x.IsDeleted)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return texts.ConvertAll(t => new TextObjectModel(t));
    }


    public async Task<bool> SetRating(string textId, bool isLike, string userId)
    {
        var textIdObject = new ObjectId(textId);
        var userIdObject = new ObjectId(userId);

        var text = await _texts.Find(x => x.Id == textIdObject).FirstOrDefaultAsync();
        if (text == null || userId == string.Empty) return false;

        var userRating = await _userRatings
            .Find(x => x.TextObjectId == textIdObject && x.UserId == userIdObject)
            .FirstOrDefaultAsync();

        if (userRating == null)
        {
            userRating = new UserRating
            {
                TextObjectId = textIdObject,
                UserId = userIdObject,
                IsDeleted = !isLike
            };
            await _userRatings.InsertOneAsync(userRating);
        }
        else
        {
            userRating.IsDeleted = !isLike;
            await _userRatings.ReplaceOneAsync(x => x.Id == userRating.Id, userRating);
        }

        return true;
    }
}