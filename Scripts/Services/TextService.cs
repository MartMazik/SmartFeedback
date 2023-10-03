using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;

namespace SmartFeedback.Scripts.Services;

public class TextService : ITextService
{
    public Task<TextObject?> AddOneText(TextObject textObject)
    {
        throw new NotImplementedException();
    }

    public Task<List<TextObject>> AddMoreText(List<TextObject> textObjects)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteText(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UnDeleteText(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<TextObject>> GetAllText(int count = -1)
    {
        throw new NotImplementedException();
    }
}