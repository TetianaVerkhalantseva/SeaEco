using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.Models.Project;
using SeaEco.Abstractions.Models.Stations;
using SeaEco.Services.ProjectServices;
using SeaEco.Services.StationServices;

namespace SeaEco.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IStationService _stationService;

    public ProjectController(
        IProjectService projectService,
        IStationService stationService)
    {
        _projectService = projectService;
        _stationService = stationService;
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
    
    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }
    
    [HttpGet("{prosjektId:guid}")]
    public async Task<IActionResult> GetProjectById(Guid prosjektId)
    {
        var project = await _projectService.GetProjectByIdAsync(prosjektId);
        if (project == null)
            return NotFound();
        return Ok(project);
    }
    
    // Stasjonsoperasjoner
    [HttpGet("{prosjektId:guid}/stasjon")]
    public async Task<IActionResult> GetStations(Guid prosjektId)
    {
        var stations = await _stationService.GetStationsAsync(prosjektId);
        return Ok(stations);
    }

    [HttpGet("{prosjektId:guid}/stasjon/{stasjonsid:int}")]
    public async Task<IActionResult> GetStation(Guid prosjektId, int stasjonsid)
    {
        var station = await _stationService.GetStationByIdAsync(prosjektId, stasjonsid);
        if (station == null)
            return NotFound();
        return Ok(station);
    }

    [HttpPut("{prosjektId:guid}/stasjon/{stasjonsid:int}")]
    public async Task<IActionResult> UpdateStation(Guid prosjektId, int stasjonsid, [FromBody] UpdateStationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _stationService.UpdateStationAsync(prosjektId, stasjonsid, dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}