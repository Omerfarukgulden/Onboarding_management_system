using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;
using Onboard_management_system.OnboardingDomain.Entities;
using Onboard_management_system.OnboardingInfrastructure.Context;
using BCrypt.Net;

namespace Onboard_management_system.OnboardingApplication.Services;

public class UserService : IUserService
{
    private readonly OnboardingDbContext _context;
    private readonly IMapper _mapper;

    public UserService(OnboardingDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    //tüm userları getiren method 
    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _context.Users
            .Include(u => u.Department)
            .ToListAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    // userları idlerine göre getiren method 
    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _context.Users
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.Id == id);
        return user is null ? null : _mapper.Map<UserDto>(user);
    }

    // yeni user oluşturan method
    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        if (dto.DepartmentId.HasValue)
        {
            var departmentExists = await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId.Value);
            if (!departmentExists)
                throw new KeyNotFoundException("Belirtilen departman bulunamadı.");
        }
        
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            Role = dto.Role,
            DepartmentId = dto.DepartmentId,
            // databasede şifrelerin string olarak durmaması için 
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
        {
            throw new InvalidOperationException($"Bu username ({dto.Username}) zaten mevcut.");
            // veya daha iyi: return Result.Fail("Username already exists");
        }
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return _mapper.Map<UserDto>(user);
    }

    // userları idlerine göre güncelleyen method 
    public async Task<bool> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) return false;

        _mapper.Map(dto, user);
        await _context.SaveChangesAsync();
        return true;
    }

    // userları idlerine göre silen method 
    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) return false;

        user.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    
}