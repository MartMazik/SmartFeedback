using MongoDB.Bson;
using MongoDB.Driver;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Services.MongoDB;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _users;
    
    public UserService(IMongoDatabase database)
    {
        _users = database.GetCollection<User>("user");
    }
    
    public async Task<string?> Login(UserModel userModel)
    {
        var user = await _users.Find(u => u.Email == userModel.Email && u.Password == userModel.Password).FirstOrDefaultAsync();
        return user?.Id.ToString();
    }

    public async Task<string?> Register(UserModel userModel)
    {
        var user = await _users.Find(u => u.Email == userModel.Email).FirstOrDefaultAsync();
        if (user != null) return null;
        user = new User
        {
            Username = userModel.Username,
            Email = userModel.Email,
            Password = userModel.Password
        };
        await _users.InsertOneAsync(user);
        return user.Id.ToString();
    }

    public async Task<bool> ChangePassword(UserModel userModel, string userId)
    {
        var user = await _users.Find(u => u.Email == userModel.Email).FirstOrDefaultAsync();
        if (user == null || user.Id.ToString() != userId) return false;
        user.Password = userModel.Password;
        await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        return true;
    }

    public async Task<UserModel?> GetUser(string userId)
    {
        var userIdObject = ObjectId.Parse(userId);
        var user = await _users.Find(u => u.Id == userIdObject).FirstOrDefaultAsync();
        return user == null ? null : new UserModel(user);
    }
}