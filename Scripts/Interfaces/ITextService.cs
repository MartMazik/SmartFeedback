using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Interfaces;

public interface ITextService
{
    public Task<TextObjectModel?> AddOneText(TextObjectModel textObjectModel);
    public Task<List<TextObjectModel>> AddMoreText(List<TextObjectModel> textObjectModels);
    public Task<bool> DeleteText(string textId);
    public Task<bool> UnDeleteText(string textId);
    public Task<TextObjectModel?> UpdateText(TextObjectModel textObjectModel);
    public Task<TextObjectModel?> GetText(string textId);
    public Task<List<TextObjectModel>> GetProjectsTexts(string projectId, int page = 1, int pageSize = 10);
    public Task<bool> UploadTexts(IFormFile csvFile, string projectId);
}