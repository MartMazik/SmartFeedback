using Microsoft.AspNetCore.Mvc;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Controllers;

[Route("api/search")]
[ApiController]
public class SearchTextsController : ControllerBase
{
    private readonly ISearchTextsService _searchTextsService;

    public SearchTextsController(ISearchTextsService searchTextsService)
    {
        _searchTextsService = searchTextsService;
    }
    
    [HttpGet("{textId:int}")]
    public async Task<List<TextObjectModel>> GetSimilarTexts(int textId)
    {
        return await _searchTextsService.GetSimilarTextsById(textId);
    }
    
    [HttpPost]
    public async Task<List<TextObjectModel>> GetSimilarTexts(TextObjectModel textObjectModel)
    {
        return await _searchTextsService.GetSimilarTexts(textObjectModel);
    } 
}