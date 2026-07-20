using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;
using Onboard_management_system.OnboardingDomain.Entities;
using Onboard_management_system.OnboardingInfrastructure.Context;

namespace Onboard_management_system.OnboardingApplication.Services;

public class DepartmentService : IDepartmentService
{
    private readonly OnboardingDbContext _context;
    private readonly IMapper _mapper;

    public DepartmentService(OnboardingDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //tüm departmentleri getiren method 
    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _context.Departments.ToListAsync();
        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }
//idye göre getiren method 
    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        return department is null ? null : _mapper.Map<DepartmentDto>(department);
    }
//yeni department üreten method
    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        var department = _mapper.Map<Department>(dto);
        department.IsActive = true;
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return _mapper.Map<DepartmentDto>(department);
    }
//departmenı güncelleyen method 
    public async Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        if (department is null) return false;

        _mapper.Map(dto, department);
        await _context.SaveChangesAsync();
        return true;
    }
//departmanı siler aktifliğini false yapar 
    public async Task<bool> DeleteAsync(int id)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        if (department is null) return false;

        department.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}