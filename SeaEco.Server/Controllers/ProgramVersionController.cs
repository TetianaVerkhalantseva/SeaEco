using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.Models.ProgramVersion;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Services.ProgramVersionServices;

namespace SeaEco.Server.Controllers;


[Authorize]
[Route("/api/version")]
public class ProgramVersionController(IProgramVersionService versionService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> CurrentVersion()
    {
        Response<ProgramVersionDto> result = await versionService.CurrentVersion();
        return result.IsError
            ? AsBadRequest(result.ErrorMessage)
            : AsOk(result.Value);
    }
    
    [HttpGet("{version}")]
    public async Task<IActionResult> CurrentVersion(string version)
    {
        Response<ProgramVersionDto> result = await versionService.GetVersion(version);
        return result.IsError
            ? AsBadRequest(result.ErrorMessage)
            : AsOk(result.Value);
    }
    
    [HttpGet("list")]
    public async Task<IActionResult> Versions() => AsOk(await versionService.GetVersions());
}