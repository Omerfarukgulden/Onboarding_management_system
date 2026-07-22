using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;

namespace Onboard_management_system.OnboardingAPI.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Ik")]
public class OnboardingTemplatesController : ControllerBase
{
    private readonly IOnboardingTemplateService _service;

    public OnboardingTemplatesController(IOnboardingTemplateService service)
    {
        _service = service;
    }
//swagger ekranında taskların templateni görüntüler
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var template = await _service.GetByIdAsync(id);
        return template is null ? NotFound(new {message = $"Id'si  {id} olan templates bulunamadı ."}) : Ok(template);
    }
//swgagger ekranında template oluşturur
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOnboardingTemplateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
//swaggger ekranında templateleri günceller
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOnboardingTemplateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound(new {message = $"Id'si  {id} olan templates bulunamadı ."});
    }
//swagger ekranında templateleri siler 
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound(new {message = $"Id'si  {id} olan templates bulunamadı ."});
    }
//swagger ekranınsa task idsine göre template ekler 
    [HttpPost("{id:int}/tasks")]
    public async Task<IActionResult> AddTask(int id, [FromBody] CreateOnboardingTemplateTaskDto dto)
    {
        
            var created = await _service.AddTaskAsync(id, dto);
            return Ok(created);
    }
//swagger ekranında task idsine göre görüntüler
    [HttpGet("{id:int}/tasks")]
    public async Task<IActionResult> GetTasks(int id) => Ok(await _service.GetTasksAsync(id));
}