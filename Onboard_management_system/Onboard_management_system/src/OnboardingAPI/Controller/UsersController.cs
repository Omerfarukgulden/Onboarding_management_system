using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;

namespace Onboard_management_system.OnboardingAPI.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]

public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }
//swagger ekranında tüm userları getirir
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
//swagger ekranında idye göre userları getirir
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _service.GetByIdAsync(id);
        return user is null ? NotFound(new {message = $"Id'si  {id} olan user bulunamadı ."}) : Ok(user);
    }
//swagger ekranında yeni user ekleme işlemini yapar
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
//swagger ekranında userların idsine göre güncelleme işlemi yapar
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound(new {message = $"Id'si  {id} olan user bulunamadı ."});
    }
//swagger ekranında userları idlerine göre siler 
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound(new {message = $"Id'si  {id} olan user bulunamadı ."});
    }
}