using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services.MongoDB;

public class TextObjectService : ITextService
{
    public Task<TextObjectModel?> AddOneText(TextObjectModel textObjectModel)
    {
        throw new NotImplementedException();
    }

    public Task<List<TextObjectModel>> AddMoreText(List<TextObjectModel> textObjectModels)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteText(int textId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UnDeleteText(int textId)
    {
        throw new NotImplementedException();
    }

    public Task<TextObjectModel?> UpdateText(TextObjectModel textObjectModel)
    {
        throw new NotImplementedException();
    }

    public Task<TextObjectModel?> GetText(int textId)
    {
        throw new NotImplementedException();
    }

    public Task<List<TextObjectModel>> GetProjectsTexts(int projectId)
    {
        throw new NotImplementedException();
    }
}