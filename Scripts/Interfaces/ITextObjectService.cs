using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Interfaces;

public interface ITextObjectService
{
    public Task<TextObjectModel?> CreateOneText(TextObjectModel textObjectModel, string userId);
    public Task<List<TextObjectModel>> CreateFewText(List<TextObjectModel> textObjectModels, string userId);
    public Task<bool> UploadTexts(IFormFile csvFile, string projectId, string userId);
    public Task<bool> DeleteText(string textId, string userId);
    public Task<bool> UnDeleteText(string textId, string userId);
    public Task<TextObjectModel?> UpdateText(TextObjectModel textObjectModel, string userId);
    public Task<TextObjectModel?> GetText(string textId);
    public Task<List<TextGroupModel>> GetTextsByProject(string projectId, int page = 1, int pageSize = 20);

    public Task<List<TextGroupModel>?> SearchTexts(string projectId, string searchString, int page = 1,
        int pageSize = 20);

    public Task<TextGroupModel?> GetTextsFromGroup(string groupId, int page = 1, int pageSize = 20);

    // public Task<List<TextObjectModel>> GetSimilarTexts(string textId, int page = 1, int pageSize = 20);

    public Task<bool> SetRating(string textId, bool isLike, string userId);
}