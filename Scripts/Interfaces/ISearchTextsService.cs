using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Interfaces;

public interface ISearchTextsService
{
    public Task<List<TextObjectModel>> GetSimilarTexts(TextObjectModel textObjectModel, int page = 1, int pageSize = 10);
    public Task<List<TextObjectModel>> GetSimilarTextsById(string textId, int page = 1, int pageSize = 10);
}