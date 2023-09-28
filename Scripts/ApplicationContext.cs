using Microsoft.EntityFrameworkCore;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts;

public class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<TextObject> TextObjects { get; set; } = null!;
    public DbSet<UserRating> UserRatings { get; set; } = null!;
    public DbSet<ConnectTextsObjects> ConnectTextsObjects { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=smartfeedback;Username=postgres;Password=2560");
    }
}