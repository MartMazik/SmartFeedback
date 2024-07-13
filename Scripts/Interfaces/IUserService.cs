using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Interfaces;

public interface IUserService
{
    public Task<string?> Login(UserModel userModel);
    public Task<string?> Register(UserModel userModel);
    public Task<bool> ChangePassword(UserModel userModel, string userId);
    public Task<UserModel?> GetUser(string userId);
}