using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;
using Onboard_management_system.OnboardingDomain.Entities;
using Onboard_management_system.OnboardingInfrastructure.Context;

namespace Onboard_management_system.OnboardingApplication.Services;

public class OnboardingTemplateService : IOnboardingTemplateService
{
    private readonly OnboardingDbContext _context;
    private readonly IMapper _mapper;

    public OnboardingTemplateService(OnboardingDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    //tüm şablonları getiren method 
    public async Task<IEnumerable<OnboardingTemplateDto>> GetAllAsync()
    {
        var templates = await _context.OnboardingTemplates
            .Include(t => t.TemplateTasks)
            .ThenInclude(tt => tt.ResponsibleDepartment)
            .ToListAsync();

        return _mapper.Map<IEnumerable<OnboardingTemplateDto>>(templates);
    }
// idlere göre şablonları getiren method 
    public async Task<OnboardingTemplateDto?> GetByIdAsync(int id)
    {
        var template = await _context.OnboardingTemplates
            .Include(t => t.TemplateTasks)
            .ThenInclude(tt => tt.ResponsibleDepartment)
            .FirstOrDefaultAsync(t => t.Id == id);

        return template is null ? null : _mapper.Map<OnboardingTemplateDto>(template);
    }
//yeni şablon oluşturan method 
    public async Task<OnboardingTemplateDto> CreateAsync(CreateOnboardingTemplateDto dto)
    {
        var template = _mapper.Map<OnboardingTemplate>(dto);
        template.IsActive = true;
        _context.OnboardingTemplates.Add(template);
        await _context.SaveChangesAsync();
        return _mapper.Map<OnboardingTemplateDto>(template);
    }
//şablonları idlerine göre güncelleyen method 
    public async Task<bool> UpdateAsync(int id, UpdateOnboardingTemplateDto dto)
    {
        var template = await _context.OnboardingTemplates.FirstOrDefaultAsync(t => t.Id == id);
        if (template is null) return false;

        _mapper.Map(dto, template);
        await _context.SaveChangesAsync();
        return true;
    }

//templatleri idilerine göre silen method 
    public async Task<bool> DeleteAsync(int id)
    {
        var template = await _context.OnboardingTemplates.FirstOrDefaultAsync(t => t.Id == id);
        if (template is null) return false;

        template.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    //şablonlara task ekleyen method 
    public async Task<OnboardingTemplateTaskDto> AddTaskAsync(int templateId, CreateOnboardingTemplateTaskDto dto)
    {
        var template = await _context.OnboardingTemplates.FirstOrDefaultAsync(t => t.Id == templateId);
        if (template is null)
            throw new KeyNotFoundException("Şablon bulunamadı.");

        var task = _mapper.Map<OnboardingTemplateTask>(dto);
        task.OnboardingTemplateId = templateId;

        _context.OnboardingTemplateTasks.Add(task);
        await _context.SaveChangesAsync();

        var saved = await _context.OnboardingTemplateTasks
            .Include(tt => tt.ResponsibleDepartment)
            .FirstAsync(tt => tt.Id == task.Id);

        return _mapper.Map<OnboardingTemplateTaskDto>(saved);
    }

    // şablonlara eklenen taskları gösteren method
    public async Task<IEnumerable<OnboardingTemplateTaskDto>> GetTasksAsync(int templateId)
    {
        var tasks = await _context.OnboardingTemplateTasks
            .Include(tt => tt.ResponsibleDepartment)
            .Where(tt => tt.OnboardingTemplateId == templateId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<OnboardingTemplateTaskDto>>(tasks);
    }
}