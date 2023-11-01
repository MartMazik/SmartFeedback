using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Interfaces;

public interface ITextService
{
    public Task<TextObjectModel?> AddOneText(TextObjectModel textObjectModel);
    public Task<List<TextObjectModel>> AddMoreText(List<TextObjectModel> textObjectModels);
    public Task<bool> DeleteText(int textId);
    public Task<bool> UnDeleteText(int textId);
    public Task<TextObjectModel?> UpdateText(TextObjectModel textObjectModel);
    public Task<TextObjectModel?> GetText(int textId);
    public Task<List<TextObjectModel>> GetProjectsTexts(int projectId);
}