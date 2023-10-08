using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Interfaces;

public interface ISearchTextsService
{
    public Task<TextObject?> GetText(int id);
    public Task<TextObject?> GetText(TextProjectModel textProjectModel);
}