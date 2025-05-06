using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.Models.PreInfo;
using SeaEco.Services.PreInfo;

namespace SeaEco.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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

        var newId = await _service.CreatePreInfoAsync(dto);

        // 201 Created → GET /api/PreInfo/{newId}
        return CreatedAtAction(
            nameof(GetById),
            new { preInfoId = newId },
            null
        );
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

        var ok = await _service.EditPreInfoAsync(preInfoId, dto);
        if (!ok) return NotFound();
        return NoContent();
    }
}