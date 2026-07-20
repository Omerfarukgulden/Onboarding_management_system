using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;
using Onboard_management_system.OnboardingDomain.Entities;
using Onboard_management_system.OnboardingDomain.Enums;
using Onboard_management_system.OnboardingInfrastructure.Context;

namespace Onboard_management_system.OnboardingApplication.Services;

public class OnboardingProcessService : IOnboardingProcessService
{
    private readonly OnboardingDbContext _context;
    private readonly IMapper _mapper;

    public OnboardingProcessService(OnboardingDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //işe alışım sürecini getiren method 
    public async Task<IEnumerable<OnboardingProcessDto>> GetAllAsync()
    {
        var processes = await _context.OnboardingProcesses
            .Include(p => p.Employee)
            .Include(p => p.OnboardingTemplate)
            .ToListAsync();

        return _mapper.Map<IEnumerable<OnboardingProcessDto>>(processes);
    }

    // işe alışım sürecini idye göre getiren method 
    public async Task<OnboardingProcessDto?> GetByIdAsync(int id)
    {
        var process = await _context.OnboardingProcesses
            .Include(p => p.Employee)
            .Include(p => p.OnboardingTemplate)
            .FirstOrDefaultAsync(p => p.Id == id);

        return process is null ? null : _mapper.Map<OnboardingProcessDto>(process);
    }

    // taskı başlatan method 
    public async Task<OnboardingProcessDto> StartAsync(StartOnboardingProcessDto dto)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmpId == dto.EmployeeId);
        if (employee is null)
            throw new KeyNotFoundException("Çalışan bulunamadı.");

        var template = await _context.OnboardingTemplates
            .Include(t => t.TemplateTasks)
            .FirstOrDefaultAsync(t => t.Id == dto.OnboardingTemplateId && t.IsActive);

        // taskın şablonu yoksa task başlatılamaz 
        if (template is null)
            throw new InvalidOperationException("Geçerli bir onboarding şablonu bulunamadı.");

        var process = new OnboardingProcess
        {
            EmployeeId = employee.EmpId,
            OnboardingTemplateId = template.Id,
            StartDate = DateTime.UtcNow,
            Status = OnboardingStatus.InProgress
        };

        _context.OnboardingProcesses.Add(process);
        await _context.SaveChangesAsync();

        
        foreach (var templateTask in template.TemplateTasks)
        {
            var task = new OnboardingTask
            {
                OnboardingProcessId = process.Id,
                Title = templateTask.Title,
                Description = templateTask.Description,
                ResponsibleDepartmentId = templateTask.ResponsibleDepartmentId,
                DueDate = employee.HireDate.AddDays(templateTask.DueInDays),
                Status = OnboardingDomain.Enums.TaskStatus.Pending,
                IsMandatory = templateTask.IsMandatory
            };

            _context.OnboardingTasks.Add(task);
        }

        await _context.SaveChangesAsync();

        var saved = await _context.OnboardingProcesses
            .Include(p => p.Employee)
            .Include(p => p.OnboardingTemplate)
            .FirstAsync(p => p.Id == process.Id);

        return _mapper.Map<OnboardingProcessDto>(saved);
    }
}