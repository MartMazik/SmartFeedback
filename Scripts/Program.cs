using MongoDB.Driver;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Services;
using SmartFeedback.Scripts.Services.MongoDB;
using SmartFeedback.Scripts.Services.Process;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллеры и связанные службы
builder.Services.AddControllers();

// Добавляем поддержку генерации документации Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настройка подключения к MongoDB
const string connectionString = "mongodb://localhost:27017/"; // TODO builder.Configuration.GetConnectionString("MongoDb");
const string databaseName = "smart_feedback"; // TODO builder.Configuration.GetConnectionString("DatabaseName");

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
builder.Services.AddScoped<IMongoDatabase>(_ =>
{
    var mongoClient = _.GetService<IMongoClient>();
    return mongoClient.GetDatabase(databaseName);
});

// Добавляем ваш сервис MongoDB
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITextService, TextObjectService>();
builder.Services.AddScoped<ITextRatingService, TextRatingService>();
builder.Services.AddScoped<ISearchTextsService, SearchTextsService>();
builder.Services.AddScoped<ITextProcessService, TextProcessService>();





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