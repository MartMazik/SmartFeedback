using Microsoft.AspNetCore.Mvc;
using SmartFeedback.Scripts.Entities;
using SmartFeedback.Scripts.Interfaces;

namespace SmartFeedback.Scripts.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<List<Project>> Get()
    {
        return await _projectService.GetAllProject();
    }

    [HttpGet("{id:int}")]
    public async Task<Project?> Get(int id)
    {
        return await _projectService.GetProject(id);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Project project)
    {
        var temp = await _projectService.AddProject(project);
        if (temp == null) return BadRequest();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put(Project project)
    {
        var temp = await _projectService.UpdateProject(project);
        if (temp == null) return BadRequest();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var temp = await _projectService.DeleteProject(id);
        if (!temp) return BadRequest();
        return Ok();
    }

    [HttpPut("undelete/{id}")]
    public async Task<IActionResult> UnDelete(int id)
    {
        var temp = await _projectService.UnDeleteProject(id);
        if (!temp) return BadRequest();
        return Ok();
    }
}