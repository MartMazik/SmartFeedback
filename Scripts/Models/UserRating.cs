using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartFeedback.Scripts.Interfaces;

namespace SmartFeedback.Scripts.Models;

public class UserRating : IModel
{
    [Key] public int Id { get; set; }
    public bool IsDeleted { get; set; }
    [MaxLength(512)] [Required] public string UserId { get; set; }

    [Required] public bool IsLike { get; set; }

    [ForeignKey("TextObjectId")]
    [Required]
    public TextObject TextObject { get; set; }


    public UserRating()
    {
    }

    public UserRating(string userId, bool isLike, TextObject textObject)
    {
        UserId = userId;
        IsLike = isLike;
        TextObject = textObject;
    }
}