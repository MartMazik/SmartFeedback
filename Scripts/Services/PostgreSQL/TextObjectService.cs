using Microsoft.EntityFrameworkCore;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services.PostgreSQL;

public class TextObjectService : ITextService
{
    private readonly ApplicationContext _db;

    public TextObjectService(ApplicationContext db)
    {
        _db = db;
    }


    public async Task<TextObjectModel?> AddOneText(TextObjectModel textObjectModel)
    {
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == textObjectModel.ProjectId);
        if (project == null) return null;
        var newTextObject = new TextObject(textObjectModel.Content, project);
        await _db.TextObjects.AddAsync(newTextObject);
        await _db.SaveChangesAsync();
        return new TextObjectModel(newTextObject);
    }

    public async Task<List<TextObjectModel>> AddMoreText(List<TextObjectModel> textObjectModels)
    {
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == textObjectModels[0].ProjectId);
        if (project == null) return new List<TextObjectModel>();
        var newTextObjects = textObjectModels.Select(textObject => new TextObject(textObject.Content, project)).ToList();
        await _db.TextObjects.AddRangeAsync(newTextObjects);
        await _db.SaveChangesAsync();
        return newTextObjects.Select(textObject => new TextObjectModel(textObject)).ToList();
    }

    public async Task<bool> DeleteText(int textId)
    {
        var textObject = await _db.TextObjects.FirstOrDefaultAsync(t => t.Id == textId);
        if (textObject == null) return false;
        textObject.IsDeleted = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnDeleteText(int textId)
    {
        var textObject = await _db.TextObjects.FirstOrDefaultAsync(t => t.Id == textId);
        if (textObject == null) return false;
        textObject.IsDeleted = false;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<TextObjectModel?> UpdateText(TextObjectModel textObjectModel)
    {
        var textObjectToUpdate = await _db.TextObjects.Include(to => to.Project).FirstOrDefaultAsync(t => t.Id == textObjectModel.Id);
        if (textObjectToUpdate == null) return null;
        textObjectToUpdate.Content = textObjectModel.Content;
        await _db.SaveChangesAsync();
        return new TextObjectModel(textObjectToUpdate);
    }

    public async Task<TextObjectModel?> GetText(int textId)
    {
        var textObject = await _db.TextObjects.Include(to => to.Project).FirstOrDefaultAsync(t => t.Id == textId);
        return textObject == null ? null : new TextObjectModel(textObject);
    }

    public async Task<List<TextObjectModel>> GetProjectsTexts(int projectId)
    {
        var textObjects = await _db.TextObjects.Include(to => to.Project).Where(x => x.Project.Id == projectId).ToListAsync();
        return textObjects.Select(x => new TextObjectModel(x)).ToList();
    }
}