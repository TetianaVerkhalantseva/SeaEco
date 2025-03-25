using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.Models.Project;
using SeaEco.Services.ProjectServices;

namespace SeaEco.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] NewProjectDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var id = await _projectService.CreateProjectAsync(dto);
            return Ok(new { prosjektId = id });
        }
        catch (KeyNotFoundException knf)
        {
            return NotFound(knf.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet("{prosjektId:guid}")]
    public async Task<IActionResult> GetProjectById(Guid prosjektId)
    {
        var project = await _projectService.GetProjectByIdAsync(prosjektId);
        if (project == null)
            return NotFound();
        return Ok(project);
    }
    
    [HttpGet("customer/{kundeId}")]
    public async Task<IActionResult> GetProjectsByCustomer(int kundeId)
    {
        var projects = await _projectService.GetProjectsByCustomerAsync(kundeId);
        return Ok(projects);
    }
}