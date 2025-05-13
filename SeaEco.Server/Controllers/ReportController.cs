using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Models.Report;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Contexts;
using SeaEco.EntityFramework.Entities;
using SeaEco.Reporter.Models;
using SeaEco.Services.ReportServices;
using SeaEco.Services.TilstandServices;
using SeaEco.Services.TilstandServices.Models;

namespace SeaEco.Server.Controllers;

[Route("api/report")]
public class ReportController(IReportService reportService, TilstandService tilstandService, AppDbContext context) : ApiControllerBase
{
    [HttpPost("generate/info")]
    public async Task<IActionResult> GenerateInfo([FromBody] Guid projectId)
    {
        Response<string> response = await reportService.GenerateInfoReport(projectId);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk(response.Value);
    }
    
    [HttpPost("generate/b1")]
    public async Task<IActionResult> GenerateB1([FromBody] Guid projectId)
    {
        Response<string> response = await reportService.GenerateB1Report(projectId);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk(response.Value);
    }

    [HttpPost("generate/b2")]
    public async Task<IActionResult> GenerateB2([FromBody] Guid projectId)
    {
        Response<string> response = await reportService.GenerateB2Report(projectId);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk(response.Value);
    }

    [HttpPost("generate/positions")]
    public async Task<IActionResult> GeneratePositions([FromBody] Guid projectId)
    {
        Response<string> response = await reportService.GeneratePositionsReport(projectId);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk(response.Value);
    }
    
    [HttpPost("generate/images")]
    public async Task<IActionResult> GenerateImages([FromBody] Guid projectId)
    {
        Response<string> response = await reportService.GenerateImagesReport(projectId);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk(response.Value);
    }
    
    [HttpPost("generate/pt-plan")]
    public async Task<IActionResult> GeneratePtp([FromBody] Guid projectId)
    {
        Response<string> response = await reportService.GeneratePtpReport(projectId);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk(response.Value);
    }
    
    [HttpPost("generate/all")]
    public async Task<IActionResult> GenerateAll([FromBody] Guid projectId)
    {
        Response<IEnumerable<Response<string>>> response = await reportService.GenerateAllReports(projectId);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk(response.Value.Select(_ => new
            {
                ErrorMessage = _.IsError ? _.ErrorMessage : null,
                Path = _.IsError ? null : _.Value
            }));
    }

    [HttpGet("{projectId:guid}/all")]
    public async Task<IActionResult> GetAll([FromRoute] Guid projectId) => AsOk(await reportService.GetAllReports(projectId));

    [HttpGet("{projectId:guid}/pt-plan")]
    public async Task<IActionResult> GetPtp([FromRoute] Guid projectId)
    {
        Response<ReportDto> response = await reportService.GetPtpReport(projectId);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk(response.Value);
    }
    
    [HttpGet("{reportId:guid}")]
    public async Task<IActionResult> DownloadReport([FromRoute] Guid reportId)
    {
        Response<FileModel> result = await reportService.DownloadReportById(reportId);
        if (result.IsError)
        {
            return AsBadRequest(result.ErrorMessage);
        }
        
        return File(result.Value.Content, result.Value.ContentType, result.Value.DownloadName);
    }
    
    // De neste endpoints for TilstandServices – skal ikke brukes på Client
    [HttpPost("klasse")]
    public async Task<IActionResult> GenerateClass([FromBody] CalculateClassModel model)
    {
        try
        {
            var result = tilstandService.CalculateClass(model.pH, model.eH);
            return AsOk(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return AsBadRequest(e.ToString());
        }
    }

    [HttpPut("tilstand/sediment/{id:guid}")]
    public async Task<IActionResult> CalculateSediment([FromRoute] Guid id)
    {
        BSediment? sediment = await context.BSediments.FirstOrDefaultAsync(_ => _.Id == id);
        if (sediment is null)
        {
            return NotFound();
        }
        
        tilstandService.CalculateSedimentTilstand(sediment);
        
        return AsOk(sediment);
    }
    
    [HttpPut("tilstand/sensorisk/{id:guid}")]
    public async Task<IActionResult> CalculateSensorisk([FromRoute] Guid id)
    {
        BSensorisk? sensorisk = await context.BSensorisks.FirstOrDefaultAsync(_ => _.Id == id);
        if (sensorisk is null)
        {
            return NotFound();
        }
        
        tilstandService.CalculateSensoriskTilstand(sensorisk);
        
        return AsOk(sensorisk);
    }
    
    [HttpPut("tilstand/undersokelses/{id:guid}")]
    public async Task<IActionResult> CalculateUndersokelseTilstand([FromRoute] Guid id)
    {
        BUndersokelse? undersokelse = await context.BUndersokelses
            .Include(_ => _.Sediment)
            .Include(_ => _.Sensorisk)
            .FirstOrDefaultAsync(_ => _.Id == id);
        
        if (undersokelse is null)
        {
            return NotFound();
        }
        
        tilstandService.CalculateUndersokelseTilstand(undersokelse);
        
        return AsOk(undersokelse);
    }
    
    [HttpPut("tilstand/project/{id:guid}")]
    public async Task<IActionResult> CalculateProject([FromRoute] Guid id)
    {
        BProsjekt? prosjekt = await context.BProsjekts
            .Include(_ => _.BUndersokelses)
            .ThenInclude(_ => _.Sensorisk)
            .Include(_ => _.BUndersokelses)
            .ThenInclude(_ => _.Sediment)
            .FirstOrDefaultAsync(_ => _.Id == id);
        
        if (prosjekt is null)
        {
            return NotFound();
        }
        
        return AsOk(tilstandService.CalculateProsjektTilstand(prosjekt.BUndersokelses, prosjekt.Id));
    }
}