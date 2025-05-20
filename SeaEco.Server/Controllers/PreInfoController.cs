using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.Models.PreInfo;
using SeaEco.Services.PreInfo;

namespace SeaEco.Server.Controllers;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PreInfoController : ControllerBase
{
    private readonly IPreInfoService _service;

    public PreInfoController(IPreInfoService service)
    {
        _service = service;
    }
    
    [HttpGet("project/{prosjektId:guid}")]
    public async Task<ActionResult<List<PreInfoDto>>> GetAllByProject(Guid prosjektId)
    {
        var list = await _service.GetAllByProjectAsync(prosjektId);
        if (list == null || !list.Any())
            return NotFound();
        return Ok(list);
    }
    
    [HttpGet("{preInfoId:guid}")]
    public async Task<ActionResult<PreInfoDto>> GetById(Guid preInfoId)
    {
        var dto = await _service.GetByIdAsync(preInfoId);
        if (dto == null) return NotFound();
        return Ok(dto);
    }
    
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] AddPreInfoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var newId = await _service.CreatePreInfoAsync(dto);
            return CreatedAtAction(
                nameof(GetById),
                new { preInfoId = newId },
                null
            );
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
        catch (KeyNotFoundException knf)
        {
            return NotFound(knf.Message);
        }
    }
    
    [HttpDelete("{preInfoId:guid}")]
    public async Task<ActionResult> Delete(Guid preInfoId)
    {
        var ok = await _service.DeletePreInfoAsync(preInfoId);
        if (!ok) return NotFound();
        return NoContent();
    }
    
    [HttpPut("{preInfoId:guid}")]
    public async Task<ActionResult> Edit(Guid preInfoId, [FromBody] EditPreInfoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _service.EditPreInfoAsync(preInfoId, dto);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }
}