using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onboard_management_system.OnboardingApplication.Common;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;
using Onboard_management_system.OnboardingDomain.Entities;
using Onboard_management_system.OnboardingInfrastructure.Context;


namespace Onboard_management_system.OnboardingApplication.Services;

public class EmployeeService : IEmployeeService
{
    private readonly OnboardingDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(OnboardingDbContext context, IMapper mapper ,  ILogger<EmployeeService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }
// tüm çalışanları gösteren method 
    public async Task<PagedResult<EmployeeDto>> GetAllAsync(EmployeeFilterDto filter)
    {
        var query = _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .AsQueryable();

        if (filter.DepartmentId.HasValue)
            query = query.Where(e => e.DepartmentId == filter.DepartmentId.Value);

        if (filter.PositionId.HasValue)
            query = query.Where(e => e.PositionId == filter.PositionId.Value);

        if (filter.IsActive.HasValue)
            query = query.Where(e => e.IsActive == filter.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim().ToLower();
            query = query.Where(e =>
                e.FirstName.ToLower().Contains(search) ||
                e.LastName.ToLower().Contains(search) ||
                e.Email.ToLower().Contains(search));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(e => e.LastName)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PagedResult<EmployeeDto>
        {
            Items = _mapper.Map<List<EmployeeDto>>(items),
            TotalCount = totalCount,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };

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
        
        _logger.LogInformation("Yeni çalışan oluşturuldu. EmpId: {EmpId}, Ad: {Name}, Departman: {Department}",
            employee.EmpId, $"{employee.FirstName} {employee.LastName}", department.Name);

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
        
        _logger.LogInformation("Çalışan pasife alındı. EmpId: {EmpId}", empId);
        
        return true;
    }
}