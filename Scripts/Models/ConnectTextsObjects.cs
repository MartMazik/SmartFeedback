using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartFeedback.Scripts.Interfaces;

namespace SmartFeedback.Scripts.Models;

public class ConnectTextsObjects : IModel
{
    [Key] public int Id { get; set; }
    public bool IsDeleted { get; set; }

    [ForeignKey("FirstTextObjectId")]
    [Required]
    public TextObject FirstTextObject { get; set; }

    [ForeignKey("SecondTextObjectId")]
    [Required]
    public TextObject SecondTextObject { get; set; }

    [Required] public double Coincidence { get; set; }


    public ConnectTextsObjects()
    {
    }

    public ConnectTextsObjects(TextObject firstTextObject, TextObject secondTextObject, double coincidence)
    {
        FirstTextObject = firstTextObject;
        SecondTextObject = secondTextObject;
        Coincidence = coincidence;
    }
}