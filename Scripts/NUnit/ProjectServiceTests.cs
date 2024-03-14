using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Services.MongoDB;

namespace SmartFeedback.Scripts.NUnit;

[TestFixture]
public class ProjectServiceTests
{
    private IMongoDatabase _testSmartFeedbackDatabase;
    private ProjectService _projectService;
    
    [SetUp]
    public async Task Setup()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        _testSmartFeedbackDatabase = client.GetDatabase("testSmartFeedback");
        _projectService = new ProjectService(_testSmartFeedbackDatabase);
    }
    
    [Test]
    public async Task DeleteProject_ProjectExists_ReturnsTrue()
    {
        // Добавляем данные в БД
        var projectModel = await _projectService.AddProject("TestProject");
        
        // Act
        var result = projectModel != null && await _projectService.DeleteProject(projectModel.Id);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteProject_ProjectDoesNotExist_ReturnsFalse()
    {
        // Act
        var result = await _projectService.DeleteProject(ObjectId.GenerateNewId().ToString());

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task GetAllProject_ReturnsProjects()
    {
        // Добавляем данные в БД
        await _projectService.AddProject("TestProject1");
        await _projectService.AddProject("TestProject2");
        await _projectService.AddProject("TestProject3");
        
        // Act
        var projects = await _projectService.GetFewProjects(1, 4);
        
        Console.WriteLine(projects.Count);
        
        // Assert
        Assert.That(projects.Count, Is.EqualTo(4));
    }
    
    [Test]
    public async Task AddProject_ProjectNameIsEmpty_ReturnsNull()
    {
        // Act
        var projectName = "MyTestProject";
        var result = await _projectService.AddProject(projectName);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(projectName));
        
    }
}