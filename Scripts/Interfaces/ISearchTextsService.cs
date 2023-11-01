using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Interfaces;

public interface ISearchTextsService
{
    public Task<List<TextObjectModel>> GetSimilarTexts(TextObjectModel textObjectModel);
    public Task<List<TextObjectModel>> GetSimilarTextsById(int textId);
}