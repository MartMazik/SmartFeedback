using Microsoft.AspNetCore.Mvc;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchTextsController : ControllerBase
    {
        private readonly ISearchTextsService _searchTextsService;

        public SearchTextsController(ISearchTextsService searchTextsService)
        {
            _searchTextsService = searchTextsService;
        }

        [HttpGet("get-similar-by-id")]
        public async Task<List<TextObjectModel>> GetSimilarTexts(string textId, int page, int pageSize)
        {
            return await _searchTextsService.GetSimilarTextsById(textId, page, pageSize);
        }

        [HttpPost("get-similar-by-text")]
        public async Task<List<TextObjectModel>> GetSimilarTexts(TextObjectModel textObjectModel, int page, int pageSize)
        {
            return await _searchTextsService.GetSimilarTexts(textObjectModel, page, pageSize);
        }
    }
}