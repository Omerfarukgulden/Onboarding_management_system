using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;

namespace Onboard_management_system.OnboardingAPI.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }
   //swagger ekranı department işlerinde get işlemi 
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _departmentService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        return department is null ? NotFound() : Ok(department);
    }
// swagger ekranı department işlerinde post işlemi 
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
    {
        var created = await _departmentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
// swagger ekranı department işlerinde idye göre update işlemi 
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentDto dto)
    {
        var updated = await _departmentService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound(new {message = $"Id'si  {id} olan department bulunamadı ."});
    }
// swagger ekranı idye department işlerinde göre silme işlemi 
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _departmentService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound(new {message = $"Id'si  {id} olan department bulunamadı ."});
    }
}