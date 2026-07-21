using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;

namespace Onboard_management_system.OnboardingAPI.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize (Roles = "Ik")]
public class OnboardingProcessesController : ControllerBase
{
    private readonly IOnboardingProcessService _service;

    public OnboardingProcessesController(IOnboardingProcessService service)
    {
        _service = service;
    }
//swagger ekranında tüm işlerin durumunu getiren işlme 
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
//swagger ekranında idye göre işleri getiren işlem 
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var process = await _service.GetByIdAsync(id);
        return process is null ? NotFound() : Ok(process);
    }
//swagger ekranında yeni başlayan employeenin sürecini getirme işlemi 
    [HttpPost]
    public async Task<IActionResult> Start([FromBody] StartOnboardingProcessDto dto)
    {
        try
        {
            var created = await _service.StartAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}