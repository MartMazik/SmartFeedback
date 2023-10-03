using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartFeedback.Scripts.Interfaces;

namespace SmartFeedback.Scripts.Entities;

public class TextObject : IModel
{
    [Key] public int Id { get; set; }
    public bool IsDeleted { get; set; }
    [MaxLength(3000)] [Required] public string Text { get; set; }

    [MaxLength(3000)] public string TechnicalText { get; set; }

    [ForeignKey("ProjectId")] [Required] public Project Project { get; set; }

    public int AnalogCount { get; set; }

    public int UserRatingCount { get; set; }

    public int RatingSum { get; set; }


    public TextObject()
    {
    }

    public TextObject(string text, string technicalText, Project project, int analogCount, int userRatingCount,
        int ratingSum)
    {
        Text = text;
        TechnicalText = technicalText;
        Project = project;
        AnalogCount = analogCount;
        UserRatingCount = userRatingCount;
        RatingSum = ratingSum;
    }
}