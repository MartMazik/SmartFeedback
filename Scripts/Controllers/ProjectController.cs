using Microsoft.AspNetCore.Mvc;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Models;

namespace SmartFeedback.Scripts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("get")]
        public async Task<ProjectModel?> GetOne(string id)
        {
            return await _projectService.GetProject(id);
        }

        [HttpGet("get-few")]
        public async Task<List<ProjectModel>> GetMore(int page, int pageSize)
        {
            return await _projectService.GetAllProject(page, pageSize);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Post(ProjectModel project)
        {
            var temp = await _projectService.AddProject(project.Name);
            if (temp == null) return BadRequest();
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Put(ProjectModel project)
        {
            var temp = await _projectService.UpdateProject(project);
            if (temp == null) return BadRequest();
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var temp = await _projectService.DeleteProject(id);
            if (!temp) return BadRequest();
            return Ok();
        }

        [HttpPut("undelete")]
        public async Task<IActionResult> UnDelete(string id)
        {
            var temp = await _projectService.UnDeleteProject(id);
            if (!temp) return BadRequest();
            return Ok();
        }
    }
}