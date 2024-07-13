using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;
using SmartFeedback.Scripts.Services.Authentication;

namespace SmartFeedback.Scripts.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<string?> Login(UserModel userModel)
    {
        var userId = await _userService.Login(userModel);
        if (userId == null) return null;
        var token = JwtAuthenticationService.GenerateToken(userId);
        return token;
    }

    [HttpPost("register")]
    public async Task<string?> Register(UserModel userModel)
    {
        var userId = await _userService.Register(userModel);
        if (userId == null) return null;
        var token = JwtAuthenticationService.GenerateToken(userId);
        return token;
    }

    [HttpPost("change-password")]
    public async Task<bool> ChangePassword(UserModel userModel)
    {
        var userId = JwtAuthenticationService.GetUserIdAsync(Request);
        if (userId == null) return false;
        var success = await _userService.ChangePassword(userModel, userId);
        return success;
    }

    [Authorize]
    [HttpPost("get-user")]
    public async Task<UserModel?> GetUser()
    {
        var userId = JwtAuthenticationService.GetUserIdAsync(Request);
        if (userId == null) return null;
        var userModel = await _userService.GetUser(userId);
        return userModel;
    }
}