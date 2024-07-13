using SmartFeedback.Scripts.Entities;

namespace SmartFeedback.Scripts.Models;

public class UserModel
{
    public string Id { get; set; } = "";

    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public UserModel(User user)
    {
        Id = user.Id.ToString() ?? string.Empty;
        Username = user.Username;
        Email = user.Email;
        Password = user.Password;
    }

    public UserModel()
    {
    }
}