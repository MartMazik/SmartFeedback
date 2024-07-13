using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Interfaces;

public interface IProcessingModuleService
{
    public Task<TextObject?> PreprocessingOne(TextObject text);
    public Task<List<TextObject>> PreprocessingFew(List<TextObject> texts);
    public Task<bool> ComparisonOne(TextObject text);
    public Task<bool> ComparisonFew(List<TextObject> texts);
    
    public Task<bool> ReComparisonInProject(string projectId);
    
    // public Task<bool> ComparisonFew(Project project);
    // public Task<bool> PreprocessingComparisonOne(TextObject text);
    // public Task<bool> PreprocessingComparisonFew(Project project);
    
    public Task<List<TextGroupModel>> SearchText(string searchText, string projectId);
    public Task<List<Project>> SearchProject(string searchText);
}