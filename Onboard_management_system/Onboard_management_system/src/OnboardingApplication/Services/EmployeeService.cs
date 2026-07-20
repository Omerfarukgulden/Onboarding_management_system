using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;
using Onboard_management_system.OnboardingDomain.Entities;
using Onboard_management_system.OnboardingInfrastructure.Context;
using Onboard_management_system.OnboardingApplication.Services.exceptions;

namespace Onboard_management_system.OnboardingApplication.Services;

public class EmployeeService : IEmployeeService
{
    private readonly OnboardingDbContext _context;
    private readonly IMapper _mapper;

    public EmployeeService(OnboardingDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
// tüm çalışanları gösteren method 
    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .ToListAsync();

        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
// idlerine göre çalışan getiren method 
    public async Task<EmployeeDto?> GetByIdAsync(int empId)
    {
        var employee = await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .FirstOrDefaultAsync(e => e.EmpId == empId);

        return employee is null ? null : _mapper.Map<EmployeeDto>(employee);
    }
// yeni çalışan oluşturan method
    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == dto.DepartmentId);

        // eğer department yoksa departman bulunamadı hatası verir
        if (department is null)
            throw new KeyNotFoundException("Departman bulunamadı.");
        // eğer department pasifse çalışan eklenemez hatası verir
        if (!department.IsActive)
            throw new InvalidOperationException("Pasif bir departman için çalışan oluşturulamaz.");

        var employee = _mapper.Map<Employee>(dto);
        employee.IsActive = true;

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(employee.EmpId)
               ?? throw new InvalidOperationException("Çalışan oluşturuldu ama tekrar okunamadı.");
    }
    // çalışanları idlerine göre güncelleyen method 
    public async Task<bool> UpdateAsync(int empId, UpdateEmployeeDto dto)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmpId == empId);
        if (employee is null) return false;

        _mapper.Map(dto, employee);
        await _context.SaveChangesAsync();
        return true;
    }
//idsi girilen çalışanı siler 
    public async Task<bool> DeleteAsync(int empId)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmpId == empId);
        if (employee is null) return false;

        employee.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}