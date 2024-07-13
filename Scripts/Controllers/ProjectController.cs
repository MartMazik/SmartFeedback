using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;
using SmartFeedback.Scripts.Services.Authentication;

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

    [Authorize]
    [HttpPost("get")]
    public async Task<ProjectModel?> GetOne(string id)
    {
        return await _projectService.GetProject(id);
    }

    [Authorize]
    [HttpPost("get-few")]
    public async Task<List<ProjectModel>> GetFew(int page, int pageSize)
    {
        return await _projectService.GetFewProjects(page, pageSize);
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<ProjectModel?> Create(ProjectModel project)
    {
        var userId = JwtAuthenticationService.GetUserIdAsync(Request);
        if (userId == null) return null;
        project.UserId = userId;
        var temp = await _projectService.CreateProject(project);
        return temp;
    }

    [Authorize]
    [HttpPost("update")]
    public async Task<ProjectModel?> Update(ProjectModel project)
    {
        var userId = JwtAuthenticationService.GetUserIdAsync(Request);
        if (userId == null) return null;
        project.UserId = userId;
        var temp = await _projectService.UpdateProject(project);
        return temp;
    }

    [Authorize]
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(string projectId)
    {
        var userId = JwtAuthenticationService.GetUserIdAsync(Request);
        if (userId == null) return BadRequest();
        var temp = await _projectService.DeleteProject(projectId, userId);
        if (!temp) return BadRequest();
        return Ok();
    }

    [Authorize]
    [HttpPost("undelete")]
    public async Task<IActionResult> UnDelete(string projectId)
    {
        var userId = JwtAuthenticationService.GetUserIdAsync(Request);
        if (userId == null) return BadRequest();
        var temp = await _projectService.UnDeleteProject(projectId, userId);
        if (!temp) return BadRequest();
        return Ok();
    }

    [Authorize]
    [HttpPost("search")]
    public async Task<List<ProjectModel>> Search(string searchString, int page, int pageSize)
    {
        return await _projectService.SearchProjects(searchString, page, pageSize);
    }

    [Authorize]
    [HttpPost("get-user-projects")]
    public async Task<List<ProjectModel>> GetUserProjects(int page, int pageSize)
    {
        var userId = JwtAuthenticationService.GetUserIdAsync(Request);
        if (userId == null) return [];
        return await _projectService.GetUserProjects(userId, page, pageSize);
    }
}