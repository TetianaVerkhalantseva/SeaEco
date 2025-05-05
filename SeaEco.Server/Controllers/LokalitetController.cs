using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaEco.Services.LokalitetServices;

namespace SeaEco.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LokalitetController : ControllerBase
{
    private readonly ILokalitetService _lokalitetService;
    public LokalitetController(ILokalitetService lokalitetService) 
    {
        _lokalitetService = lokalitetService;
        
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _lokalitetService.GetAllAsync();
        if (!list.Any()) return NotFound("Ingen lokaliteter funnet.");
        return Ok(list);
    }
}