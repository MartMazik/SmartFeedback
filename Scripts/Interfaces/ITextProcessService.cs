namespace SmartFeedback.Scripts.Interfaces;

public interface ITextProcessService
{
    public Task<bool> UpdateTextPreprocessing(string projectId);
    public Task<bool> CompareTexts(string projectId);
}