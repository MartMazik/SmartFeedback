using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Entities;

public class User
{
    [BsonId] [BsonElement("_id")] public ObjectId Id { get; set; }

    [BsonElement("is_deleted")] public bool IsDeleted { get; set; }

    [BsonElement("username")] public string Username { get; set; }

    [BsonElement("email")] public string Email { get; set; }

    [BsonElement("password")] public string Password { get; set; }

    public User(UserModel userModel)
    {
        Username = userModel.Username;
        Email = userModel.Email;
        Password = userModel.Password;
    }

    public User()
    {
    }
}