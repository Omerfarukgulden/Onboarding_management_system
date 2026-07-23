using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;

namespace Onboard_management_system.OnboardingAPI.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles =  "Ik,Admin")]
public class EmployeesController(IEmployeeService employeeService) : ControllerBase
{
    private readonly IEmployeeService _employeeService = employeeService;

    //swagger ekranı employee işlerinde görüntüleme işlemi 
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] 
        EmployeeFilterDto filter)
        => Ok(await _employeeService.GetAllAsync(filter));
//swagger ekranı employee işlerinde employeeleri idlerine göre gçrüntüleme işlemi 
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        return employee is null ? NotFound(new {message = $"Id'si  {id} olan çalışan bulunamadı ."}) : Ok(employee);
    }
//swagger ekranı yeni employee oluşturma işlemi 
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        var created = await _employeeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.EmpId }, created);
    }
// swagger ekranı ıd ye göre employeelerin bilgilerini güncelleme işlemi 
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
    {
        var updated = await _employeeService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound(new {message = $"Id'si  {id} olan çalışan bulunamadı ."});
    }
// swagger ekranı id ye göre employelleri silme ekranı 
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _employeeService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound(new {message = $"Id'si  {id} olan çalışan bulunamadı ."});
    }
}