using System.ComponentModel.DataAnnotations;
using SmartFeedback.Scripts.Interfaces;

namespace SmartFeedback.Scripts.Entities;

public class Project : IModel
{
    [Key] public int Id { get; set; }
    public bool IsDeleted { get; set; }
    [MaxLength(50)] [Required] public string Name { get; set; }


    public Project()
    {
    }

    public Project(string name)
    {
        Name = name;
    }
}