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
            return Ok(new { Id = id });
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
    
    [HttpGet("{Id:guid}")]
    public async Task<IActionResult> GetProjectById(Guid Id)
    {
        var project = await _projectService.GetProjectByIdAsync(Id);
        if (project == null)
            return NotFound();
        return Ok(project);
    }
    
    // Stasjonsoperasjoner
    [HttpGet("{Id:guid}/stasjon")]
    public async Task<IActionResult> GetStations(Guid Id)
    {
        var stations = await _stationService.GetStationsAsync(Id);
        return Ok(stations);
    }

    [HttpGet("{Id:guid}/stasjon/{stasjonsid:int}")]
    public async Task<IActionResult> GetStation(Guid Id, Guid stasjonsid)
    {
        var station = await _stationService.GetStationByIdAsync(Id, stasjonsid);
        if (station == null)
            return NotFound();
        return Ok(station);
    }

    [HttpPut("{Id:guid}/stasjon/{stasjonsid:guid}")]
    public async Task<IActionResult> UpdateStation(Guid Id, Guid stasjonsid, [FromBody] UpdateStationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _stationService.UpdateStationAsync(Id, stasjonsid, dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("{Id:guid}/stasjon")]
    public async Task<IActionResult> AddExtraStation(Guid Id, [FromBody] NewStationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            var newStationId = await _stationService.AddExtraStationAsync(Id, dto);
            return Ok(new { Id, stasjonsid = newStationId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}