using Microsoft.AspNetCore.Mvc;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;

namespace Onboard_management_system.OnboardingAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class OnboardingTasksController : ControllerBase
{
    private readonly IOnboardingTaskService _service;

    public OnboardingTasksController(IOnboardingTaskService service)
    {
        _service = service;
    }
    [HttpGet("{id:int}/history")]
    public async Task<IActionResult> GetHistory(int id) => Ok(await _service.GetHistoryAsync(id));
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _service.GetByIdAsync(id);
        return task is null ? NotFound() : Ok(task);
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOnboardingTaskStatusDto dto)
    {
        try
        {
            var updated = await _service.UpdateStatusAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}/note")]
    public async Task<IActionResult> UpdateNote(int id, [FromBody] UpdateOnboardingTaskNoteDto dto)
    {
        var updated = await _service.UpdateNoteAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }
}
