using Microsoft.AspNetCore.Mvc;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

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
        return await _projectService.GetAll();
    }

    [HttpGet("{id:int}")]
    public async Task<Project?> Get(int id)
    {
        return await _projectService.Get(id);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Project project)
    {
        var temp = await _projectService.Add(project);
        if (temp == null) return BadRequest();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put(Project project)
    {
        var temp = await _projectService.Update(project);
        if (temp == null) return BadRequest();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var temp = await _projectService.Delete(id);
        if (!temp) return BadRequest();
        return Ok();
    }

    [HttpPut("undelete/{id}")]
    public async Task<IActionResult> UnDelete(int id)
    {
        var temp = await _projectService.UnDelete(id);
        if (!temp) return BadRequest();
        return Ok();
    }
}