using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services.MongoDB;

public class SearchTextsService : ISearchTextsService
{
    public Task<List<TextObjectModel>> GetSimilarTexts(TextObjectModel textObjectModel)
    {
        throw new NotImplementedException();
    }

    public Task<List<TextObjectModel>> GetSimilarTextsById(int textId)
    {
        throw new NotImplementedException();
    }
}