using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Interfaces;

public interface ITextService
{
    public Task<TextObject?> AddOneText(TextObject textObject);
    public Task<List<TextObject>> AddMoreText(List<TextObject> textObjects);
    public Task<bool> DeleteText(int id);
    public Task<bool> UnDeleteText(int id);
    public Task<TextObject?> UpdateText(TextObject textObject);
    public Task<TextObject?> GetText(int id);
    public Task<List<TextObject>> GetAllText(int count = -1);
}