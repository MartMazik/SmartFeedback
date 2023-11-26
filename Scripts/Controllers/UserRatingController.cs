using Microsoft.AspNetCore.Mvc;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Controllers;

[Route("api/rating")]
[ApiController]
public class UserRatingController : ControllerBase
{
    private readonly  ITextRatingService _textRatingService;

    public UserRatingController(ITextRatingService textRatingService)
    {
        _textRatingService = textRatingService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(UserRatingModel userRatingModel)
    {
        var ratingResult = await _textRatingService.SetRating(userRatingModel);
        if (!ratingResult) return BadRequest();
        return Ok();
    }
}