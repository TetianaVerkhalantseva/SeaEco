using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.Models.Project;
using SeaEco.Abstractions.Models.SamplingPlan;
using SeaEco.Abstractions.Models.Stations;
using SeaEco.Server.Infrastructure;
using SeaEco.Services.ProjectServices;
using SeaEco.Services.SamplingPlanServices;
using SeaEco.Services.StationServices;

namespace SeaEco.Server.Controllers;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IStationService _stationService;
    private readonly ISamplingPlanService _samplingPlanService;

    public ProjectController(
        IProjectService projectService,
        IStationService stationService,
        ISamplingPlanService samplingPlanService)
    {
        _projectService = projectService;
        _stationService = stationService;
        _samplingPlanService = samplingPlanService;
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
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProject(Guid id, [FromBody] EditProjectDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _projectService.UpdateProjectAsync(id, dto);
            return Ok();
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
    
    // Operasjoner for prøvtakningsplan
    [HttpGet("{projectId:guid}/sampling-plan/{samplingPlanId:guid}")]
    public async Task<IActionResult> GetProjectSamplingPlan(Guid projectId, Guid samplingPlanId)
    {
        var samplingPlan = await _samplingPlanService.GetSamplingPlanById(samplingPlanId);
        if (samplingPlan == null)
        {
            return NotFound($"No sampling plan found with id {samplingPlanId}");
        }

        if (samplingPlan.ProsjektId != projectId)
        {
            return BadRequest("Sampling plan does not belong to the given project");
        }
        
        return Ok(samplingPlan);
    }

    [RoleAccessor(true)]
    [HttpPost("{projectId:guid}/sampling-plan")]
    public async Task<IActionResult> CreateSamplingPlan([FromRoute] Guid projectId,
        [FromBody] EditSamplingPlanDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        dto.ProsjektId = projectId;
        var result = await _samplingPlanService.CreateSamplingPlan(dto);
        
        return result.IsSuccess ? Ok(result.Message) : BadRequest(result.Message);
    }

    [RoleAccessor(true)]
    [HttpPut("{projectId:guid}/sampling-plan/{samplingPlanId:guid}")]
    public async Task<IActionResult> UpdateSamplingPlan([FromRoute] Guid projectId,
        [FromRoute] Guid samplingPlanId, [FromBody] EditSamplingPlanDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        dto.ProsjektId = projectId;
        var result = await _samplingPlanService.UpdateSamplingPlan(samplingPlanId, dto);
        
        return result.IsSuccess ? Ok(result.Message) : BadRequest(result.Message);
    }

    [RoleAccessor(true)]
    [HttpDelete("{projectId:guid}/sampling-plan/{samplingPlanId:guid}")]
    public async Task<IActionResult> DeleteSamplingPlan(Guid projectId, Guid samplingPlanId)
    {
        var result = await _samplingPlanService.DeleteSamplingPlan(projectId, samplingPlanId);
        return result.IsSuccess ? Ok(result.Message) : BadRequest(result.Message);
    }
}