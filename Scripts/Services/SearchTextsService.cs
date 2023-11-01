using Microsoft.EntityFrameworkCore;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services;

public class SearchTextsService : ISearchTextsService
{
    private readonly ApplicationContext _db;

    public SearchTextsService(ApplicationContext db)
    {
        _db = db;
    }
    
    
    
    public async Task<List<TextObjectModel>> GetSimilarTexts(TextObjectModel textObjectModel)
    {
        // TODO: Сделать поиск похожих текстов
        // На данный момент - поиск всех текстов по проекту
        var textObjects = await _db.TextObjects.Include(to => to.Project)
            .Where(x => x.Project.Id == textObjectModel.ProjectId).ToListAsync();
        return textObjects.Select(x => new TextObjectModel(x)).ToList();
    }

    public async Task<List<TextObjectModel>> GetSimilarTextsById(int textId)
    {
        // TODO: Сделать поиск похожих текстов к тексту с id = textId
        var textObject = await _db.TextObjects.Include(to => to.Project).FirstOrDefaultAsync(x => x.Id == textId);
        if (textObject == null) return new List<TextObjectModel>();
        var textObjects = await _db.TextObjects.Where(x => x.Project.Id == textObject.Project.Id).ToListAsync();
        return textObjects.Select(x => new TextObjectModel(x)).ToList();
    }
}