using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Services.ReportServices;

namespace SeaEco.Server.Controllers;

[Route("api/report")]
public class ReportController(IReportService reportService) : ApiControllerBase
{
    [HttpPost("generate/info")]
    [AllowAnonymous]
    public async Task<IActionResult> GenerateInfo([FromBody] Guid projectId)
    {
        Response<string> response = await reportService.GenerateInfoReport(projectId);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk(response.Value);
    }
    
    [HttpPost("generate/b1")]
    [AllowAnonymous]
    public async Task<IActionResult> GenerateB1([FromBody] Guid projectId)
    {
        Response<string> response = await reportService.GenerateB1Report(projectId);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk(response.Value);
    }

    [HttpPost("generate/b2")]
    [AllowAnonymous]
    public async Task<IActionResult> GenerateB2([FromBody] Guid projectId)
    {
        Response<string> response = await reportService.GenerateB2Report(projectId);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk(response.Value);
    }
}