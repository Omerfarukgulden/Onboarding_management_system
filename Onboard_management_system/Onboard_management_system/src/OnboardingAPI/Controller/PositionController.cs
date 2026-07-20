using Microsoft.AspNetCore.Mvc;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;

namespace Onboard_management_system.OnboardingAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class PositionsController : ControllerBase
{
    private readonly IPositionService _positionService;

    public PositionsController(IPositionService positionService)
    {
        _positionService = positionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _positionService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var position = await _positionService.GetByIdAsync(id);
        return position is null ? NotFound() : Ok(position);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePositionDto dto)
    {
        var created = await _positionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePositionDto dto)
    {
        var updated = await _positionService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _positionService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}