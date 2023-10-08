using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services;

public class SearchTextsService : ISearchTextsService
{
    public Task<TextObject?> GetText(int id)
    {
        throw new NotImplementedException();
    }

    public Task<TextObject?> GetText(TextProjectModel textProjectModel)
    {
        throw new NotImplementedException();
    }
}