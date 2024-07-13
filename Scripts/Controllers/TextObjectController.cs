using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;
using SmartFeedback.Scripts.Services.Authentication;

namespace SmartFeedback.Scripts.Controllers;

[Route("api/text")]
[ApiController]
public class TextObjectController : ControllerBase
{
    private readonly ITextObjectService _textObjectService;
    private readonly IProcessingModuleService _processingModuleService;

    public TextObjectController(ITextObjectService textObjectService, IProcessingModuleService processingModuleService)
    {
        _textObjectService = textObjectService;
        _processingModuleService = processingModuleService;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<string> Create(TextObjectModel textObject)
    {
        var userId = JwtAuthenticationService.GetUserIdAsync(Request);
        if (userId == null) return string.Empty;
        var textObjectModel = await _textObjectService.CreateOneText(textObject, userId);
        return textObjectModel == null ? string.Empty : textObjectModel.Id;
    }

    [Authorize]
    [HttpPost("create-few")]
    public async Task<IActionResult> CreateFew(List<TextObjectModel> textObjects)
    {
        var userId = JwtAuthenticationService.GetUserIdAsync(Request);
        if (userId == null) return BadRequest();
        var textObjectModels = await _textObjectService.CreateFewText(textObjects, userId);
        if (textObjectModels.Count <= 0) return BadRequest();
        return Ok();
    }

    [Authorize]
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile csvFile, string projectId)
    {
        var userId = JwtAuthenticationService.GetUserIdAsync(Request);
        if (userId == null) return BadRequest();
        var result = await _textObjectService.UploadTexts(csvFile, projectId, userId);
        if (!result) return BadRequest();
        return Ok();
    }

    [Authorize]
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(string textId)
    {
        var userId = JwtAuthenticationService.GetUserIdAsync(Request);
        if (userId == null) return BadRequest();
        var result = await _textObjectService.DeleteText(textId, userId);
        if (!result) return BadRequest();
        return Ok();
    }

    [Authorize]
    [HttpPost("undelete")]
    public async Task<IActionResult> UnDelete(string textId)
    {
        var userId = JwtAuthenticationService.GetUserIdAsync(Request);
        if (userId == null) return BadRequest();
        var result = await _textObjectService.UnDeleteText(textId, userId);
        if (!result) return BadRequest();
        return Ok();
    }

    [Authorize]
    [HttpPost("update")]
    public async Task<TextObjectModel?> Update(TextObjectModel textObject)
    {
        var userId = JwtAuthenticationService.GetUserIdAsync(Request);
        if (userId == null) return null;
        var result = await _textObjectService.UpdateText(textObject, userId);
        return result;
    }

    [Authorize]
    [HttpPost("get")]
    public async Task<TextObjectModel?> GetOne(string textId)
    {
        var textObjectModel = await _textObjectService.GetText(textId);
        return textObjectModel;
    }

    [Authorize]
    [HttpPost("get-by-project")]
    public async Task<List<TextGroupModel>> GetByProject(string projectId, int page = 1, int pageSize = 20)
    {
        var textGroupModels = await _textObjectService.GetTextsByProject(projectId, page, pageSize);
        return textGroupModels;
    }

    [Authorize]
    [HttpPost("search")]
    public async Task<List<TextGroupModel>> Search(string projectId, string searchString, int page = 1,
        int pageSize = 20)
    {
        var textGroupModels = await _textObjectService.SearchTexts(projectId, searchString, page, pageSize);
        return textGroupModels;
    }

    [Authorize]
    [HttpPost("get-by-group")]
    public async Task<TextGroupModel?> GetByGroup(string groupId, int page = 1, int pageSize = 20)
    {
        var textGroupModel = await _textObjectService.GetTextsFromGroup(groupId, page, pageSize);
        return textGroupModel;
    }

    /*[Authorize]
    [HttpPost("get-similar")]
    public async Task<List<TextObjectModel>> GetSimilar(string textId, int page = 1, int pageSize = 20)
    {
        var textObjectModels = await _textObjectService.GetSimilarTexts(textId, page, pageSize);
        return textObjectModels;
    }*/

    [Authorize]
    [HttpPost("set-rating")]
    public async Task<IActionResult> SetRating(string textId, bool isLike)
    {
        var userId = JwtAuthenticationService.GetUserIdAsync(Request);
        if (userId == null) return BadRequest();
        var result = await _textObjectService.SetRating(textId, isLike, userId);
        if (!result) return BadRequest();
        return Ok();
    }

}