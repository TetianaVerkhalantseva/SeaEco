using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.Models.BSurvey;
using SeaEco.Abstractions.Models.Project;
using SeaEco.Abstractions.Models.SamplingPlan;
using SeaEco.Abstractions.Models.Stations;
using SeaEco.Server.Infrastructure;
using SeaEco.Services.BSurveyService;
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
    private readonly IBSurveyService _surveyService;

    public ProjectController(
        IProjectService projectService,
        IStationService stationService,
        ISamplingPlanService samplingPlanService,
        IBSurveyService bSurveyService)
    {
        _projectService = projectService;
        _stationService = stationService;
        _samplingPlanService = samplingPlanService;
        _surveyService = bSurveyService;
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

    [HttpGet("Customer/{customerId:guid}")]
    public async Task<IActionResult> GetAllProjectsByCustomerId(Guid customerId)
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }
    
    
    [HttpGet("{Id:guid}")]
    public async Task<IActionResult> GetProjectById(Guid id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
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
            var updated = await _projectService.UpdateProjectAsync(id, dto);
            return Ok(updated);
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
    
    [HttpPost("{projectId:guid}/merknad/add")]
    public async Task<IActionResult> AddMerknad(Guid projectId, [FromBody] MerknadDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _projectService.AddMerknadAsync(projectId, dto.Merknad);
            return Ok();
        }
        catch (KeyNotFoundException knf)
        {
            return NotFound(knf.Message);
        }
    }
    
    [HttpPut("{projectId:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid projectId, [FromBody] UpdateStatusDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _projectService.UpdateProjectStatusAsync(projectId, dto.Status, dto.Merknad);
            return Ok();
        }
        catch (KeyNotFoundException knf)
        {
            return NotFound(knf.Message);
        }
    }
    
    
    // Operasjoner for prøvtakningsplan
    [HttpGet("{projectId:guid}/sampling-plan")]
    public async Task<IActionResult> GetProjectSamplingPlan(Guid projectId)
    {
        var samplingPlan = await _samplingPlanService.GetSamplingPlanById(projectId);
        if (samplingPlan == null)
        {
            return NotFound($"No sampling plan found with id {projectId}");
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
        
        if (!result.IsSuccess)
            return BadRequest(result.Message);

        
        var createdPlan = await _samplingPlanService.GetSamplingPlanById(dto.ProsjektId);
        if (createdPlan == null)
            return StatusCode(500, "Kunne ikke hente opprettet sampling-plan.");
        
        return CreatedAtAction(
            nameof(GetProjectSamplingPlan),
            new { projectId = projectId },
            createdPlan
        );
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
    
    // Stasjonsoperasjoner
    [HttpGet("{projectId:guid}/stations")]
    public async Task<IActionResult> GetAllStations(Guid projectId)
    {
        var result = await _stationService.GetStationsByProjectIdAsync(projectId);
        if (!result.IsSuccess)
            return NotFound(result.Message);

        return Ok(result.Stations);
    }
    
    [HttpGet("{projectId:guid}/sampling-plan/{samplingPlanId:guid}/stations")]
    public async Task<IActionResult> GetStations(Guid projectId, Guid samplingPlanId)
    {
        var result = await _stationService.GetStationsByProvetakningsplanIdAsync(samplingPlanId);
        return result.IsSuccess ? Ok(result.Stations) : NotFound(result.Message);
    }

    [HttpGet("{projectId:guid}/sampling-plan/{samplingPlanId:guid}/station/{stationId:guid}")]
    public async Task<IActionResult> GetAStation(Guid projectId, Guid samplingPlanId, Guid stationId)
    {
        var result = await _stationService.GetStationByIdAsync(projectId, stationId);
        return result.IsSuccess ? Ok(result.Station) : NotFound(result.Message);
    }

    [HttpGet("{projectId:guid}/station/{stationId:guid}")]
    public async Task<BStationDto?> GetStationById(Guid projectId, Guid stationId)
    {
        var result = await _stationService.GetBStationDtoByStationId(projectId, stationId);
        return result;
    }

    [RoleAccessor(true)]
    [HttpPut("{projectId:guid}/sampling-plan/station/{stationId:guid}")]
    public async Task<IActionResult> UpdateStation(Guid projectId, Guid stationId, [FromBody] UpdateStationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _stationService.UpdateStationAsync(stationId, dto);
        return result.IsSuccess ? Ok(result.Message) : NotFound(result.Message);
    }
    
    //Legg til ekstra stasjon i PTP
    [RoleAccessor(true)]
    [HttpPost("{projectId:guid}/sampling-plan/{samplingPlanId:guid}/station")]
    public async Task<IActionResult> AddStation(Guid projectId, Guid samplingPlanId, [FromBody] NewStationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        dto.ProsjektId = projectId;
        var result = await _stationService.AddStationToPlanAsync(samplingPlanId, dto);
        
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        
        return CreatedAtAction(
            nameof(GetAStation),
            new { projectId, samplingPlanId, stationId = result.Station.Id },
            result.Station
        );
    }
    
    // Legg til ekstra stasjon direkte på prosjekt
    [RoleAccessor(true)]
    [HttpPost("{projectId:guid}/station")]
    public async Task<IActionResult> AddStationToProject(Guid projectId, [FromBody] NewStationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        dto.ProsjektId = projectId;
        var result = await _stationService.AddStationToProjectAsync(projectId, dto);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        var stationDto = result.Station!;
        
        return CreatedAtAction(
            nameof(GetStationById),
            new { projectId = projectId, stationId = stationDto.Id },
            stationDto
        );
    }
    
    [RoleAccessor(true)]
    [HttpDelete("{projectId:guid}/sampling-plan/station/{stationId:guid}")]
    public async Task<IActionResult> DeleteStation(Guid projectId, Guid stationId)
    {
        var result = await _stationService.DeleteStationAsync(stationId);
        return result.IsSuccess ? Ok(result.Message) : BadRequest(result.Message);
    }
    
    // BUndersøkelse operasjoner
    [HttpGet("{projectId:guid}/station/{stationId:guid}/survey/{surveyId:guid}")]
    public async Task<IActionResult> GetSurvey(Guid projectId, Guid stationId, Guid surveyId)
    {
        var project = await _projectService.GetProjectByIdAsync(projectId);
        if (project == null)
            return BadRequest("Project does not exist");
        
        var station = await _stationService.GetBStationDtoByStationId(projectId, stationId);
        if (station == null)
        {
            return BadRequest("Station does not exist");
        }
        
        var survey = await _surveyService.GetSurveyById(surveyId);
        if (survey == null)
            return BadRequest($"No survey with id {surveyId}");
        
        return Ok(survey);
    }

    [RoleAccessor(true)]
    [HttpPost("{projectId:guid}/station/{stationId:guid}/survey")]
    public async Task<IActionResult> CreateSurvey(Guid projectId, Guid stationId, [FromBody] EditSurveyDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var project = await _projectService.GetProjectByIdAsync(projectId);
        if (project == null)
            return BadRequest("Project does not exist");
        
        var station = await _stationService.GetBStationDtoByStationId(projectId, stationId);
        if (station == null)
        {
            return BadRequest("Station does not exist");
        }
        
        var result = await _surveyService.CreateSurvey(projectId, stationId, dto);
        return result.IsSuccess ? Ok(result.Message) : BadRequest(result.Message);
    }

    [RoleAccessor(true)]
    [HttpPut("{projectId:guid}/station/{stationId:guid}/survey/{surveyId:guid}")]
    public async Task<IActionResult> UpdateSurvey(Guid projectId, Guid stationId, Guid surveyId,
        [FromBody] EditSurveyDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var project = await _projectService.GetProjectByIdAsync(projectId);
        if (project == null)
            return BadRequest("Project does not exist");
        
        var station = await _stationService.GetBStationDtoByStationId(projectId, stationId);
        if (station == null)
        {
            return BadRequest("Station does not exist");
        }
        
        var survey = await _surveyService.GetSurveyById(surveyId);
        if (survey == null)
            return BadRequest($"No survey with id {surveyId}");
        
        var result = await _surveyService.UpdateSurvey(projectId, stationId, surveyId, dto);
        return result.IsSuccess ? Ok(result.Message) : BadRequest(result.Message);
    }
}