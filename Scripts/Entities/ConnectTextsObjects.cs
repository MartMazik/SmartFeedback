using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartFeedback.Scripts.Interfaces;

namespace SmartFeedback.Scripts.Entities;

public class ConnectTextsObjects : IEntity
{
    [Key] public int Id { get; set; }
    public bool IsDeleted { get; set; }

    [ForeignKey("FirstTextObjectId")]
    [Required]
    public TextObject FirstTextObject { get; set; }

    [ForeignKey("SecondTextObjectId")]
    [Required]
    public TextObject SecondTextObject { get; set; }

    [Required]
    public double Coincidence { get; set; }

    public ConnectTextsObjects()
    {
    }
}