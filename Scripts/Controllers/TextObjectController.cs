using Microsoft.AspNetCore.Mvc;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Controllers
{
    [Route("api/text")]
    [ApiController]
    public class TextObjectController : ControllerBase
    {
        private readonly ITextService _textService;
        private readonly ITextProcessService _textProcessService;

        public TextObjectController(ITextService textService,
            ITextProcessService textProcessService)
        {
            _textService = textService;
            _textProcessService = textProcessService;
        }

        [HttpGet("get-few")]
        public async Task<List<TextObjectModel>> GetAll(string projectId, int page, int pageSize)
        {
            return await _textService.GetProjectsTexts(projectId, page, pageSize);
        }

        [HttpGet("get")]
        public async Task<TextObjectModel?> Get(string id)
        {
            return await _textService.GetText(id);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddOne(TextObjectModel textObjectModel)
        {
            var temp = await _textService.AddOneText(textObjectModel);
            if (temp == null) return BadRequest();
            return Ok();
        }

        [HttpPost("add-few")]
        public async Task<IActionResult> AddMore(List<TextObjectModel> textObjectModels)
        {
            var temp = await _textService.AddMoreText(textObjectModels);
            if (temp.Count == 0) return BadRequest();
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(TextObjectModel textObjectModel)
        {
            var temp = await _textService.UpdateText(textObjectModel);
            if (temp == null) return BadRequest();
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var temp = await _textService.DeleteText(id);
            if (!temp) return BadRequest();
            return Ok();
        }

        [HttpPut("undelete")]
        public async Task<IActionResult> UnDelete(string id)
        {
            var temp = await _textService.UnDeleteText(id);
            if (!temp) return BadRequest();
            return Ok();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadTexts(IFormFile csvFile, string projectId)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                return BadRequest("ProjectId is required.");
            }

            var temp = await _textService.UploadTexts(csvFile, projectId);
            if (!temp) return BadRequest("File is not valid.");
            return Ok("File uploaded successfully.");
        }

        [HttpGet("reload_preprocessing")]
        public async Task<IActionResult> ReloadPreprocessing(string projectId)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                return BadRequest("ProjectId is required.");
            }
            var temp = await _textProcessService.UpdateTextPreprocessing(projectId);
            if (!temp) return BadRequest("Text process service error.");
            return Ok();
        }
        
        [HttpGet("reload_compare_texts")]
        public async Task<IActionResult> ReloadCompareTexts(string projectId)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                return BadRequest("ProjectId is required.");
            }
            var temp = await _textProcessService.CompareTexts(projectId);
            if (!temp) return BadRequest("Text compare texts error.");
            return Ok();
        }
    }
}