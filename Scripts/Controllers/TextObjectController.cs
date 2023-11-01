using Microsoft.AspNetCore.Mvc;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Controllers;

[Route("api/text")]
[ApiController]
public class TextObjectController : ControllerBase
{
    private readonly ITextService _textService;

    public TextObjectController(ITextService textService)
    {
        _textService = textService;
    }
    
    
    
    [HttpGet("getmore/{projectId:int}")]
    public async Task<List<TextObjectModel>> GetAll(int projectId)
    {
        return await _textService.GetProjectsTexts(projectId);
    }
    
    [HttpGet("getone/{id:int}")]
    public async Task<TextObjectModel?> Get(int id)
    {
        return await _textService.GetText(id);
    }
    
    [HttpPost("addone")]
    public async Task<IActionResult> AddOne(TextObjectModel textObjectModel)
    {
        var temp = await _textService.AddOneText(textObjectModel);
        if (temp == null) return BadRequest();
        return Ok();
    }
    
    [HttpPost("addmore")]
    public async Task<IActionResult> AddMore(List<TextObjectModel> textObjectModels)
    {
        var temp = await _textService.AddMoreText(textObjectModels);
        if (temp.Count == 0) return BadRequest();
        return Ok();
    }
    
    [HttpPut]
    public async Task<IActionResult> Put(TextObjectModel textObjectModel)
    {
        var temp = await _textService.UpdateText(textObjectModel);
        if (temp == null) return BadRequest();
        return Ok();
    }
    
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var temp = await _textService.DeleteText(id);
        if (!temp) return BadRequest();
        return Ok();
    }
    
    [HttpPut("undelete/{id:int}")]
    public async Task<IActionResult> UnDelete(int id)
    {
        var temp = await _textService.UnDeleteText(id);
        if (!temp) return BadRequest();
        return Ok();
    }
}