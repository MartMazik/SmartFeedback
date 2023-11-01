using Microsoft.EntityFrameworkCore;
using SmartFeedback.Scripts;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(
            "Host=localhost;Port=5432;Database=smartfeedback;Username=postgres;Password=2560"));
});

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITextService,TextObjectService>();
builder.Services.AddScoped<ISearchTextsService,SearchTextsService>();
builder.Services.AddScoped<ITextRatingService,TextRatingService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();