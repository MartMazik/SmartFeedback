using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Interfaces;

public interface ISearchTextsService
{
    public Task<TextObject?> GetText(int id);
    public Task<TextObject?> GetText(string content);
}

// Связь текста с проектом