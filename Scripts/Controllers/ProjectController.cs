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

    [HttpGet("getone/{id:int}")]
    public async Task<ProjectModel?> GetOne(int id)
    {
        return await _projectService.GetProject(id);
    }
    
    [HttpGet("getmore/{count:int}")]
    public async Task<List<ProjectModel>> GetMore(int count)
    {
        return await _projectService.GetAllProject(count);
    }

    [HttpPost]
    public async Task<IActionResult> Post(ProjectModel project)
    {
        var temp = await _projectService.AddProject(project.Name);
        if (temp == null) return BadRequest();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put(ProjectModel project)
    {
        var temp = await _projectService.UpdateProject(project);
        if (temp == null) return BadRequest();
        return Ok();
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var temp = await _projectService.DeleteProject(id);
        if (!temp) return BadRequest();
        return Ok();
    }

    [HttpPut("undelete/{id:int}")]
    public async Task<IActionResult> UnDelete(int id)
    {
        var temp = await _projectService.UnDeleteProject(id);
        if (!temp) return BadRequest();
        return Ok();
    }
}