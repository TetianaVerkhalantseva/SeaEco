using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Reporter.Models;
using SeaEco.Services.ReportServices;

namespace SeaEco.Server.Controllers;

[Route("api/report")]
public class ReportController(IReportService reportService) : ApiControllerBase
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
        IEnumerable<Response<string>> response = await reportService.GenerateAllReports(projectId);
        return AsOk(response.Select(_ => new
        {
            ErrorMessage = _.IsError ? _.ErrorMessage : null,
            Path = _.IsError ? null : _.Value
        }));
    }

    [HttpGet("{projectId:guid}/all")]
    public async Task<IActionResult> GetAll([FromRoute] Guid projectId) => AsOk(await reportService.GetAllReports(projectId));
    
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
}