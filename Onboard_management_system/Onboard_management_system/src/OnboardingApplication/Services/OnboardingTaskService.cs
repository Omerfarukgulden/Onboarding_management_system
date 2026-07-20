using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;
using Onboard_management_system.OnboardingDomain.Entities;
using Onboard_management_system.OnboardingDomain.Enums;
using Onboard_management_system.OnboardingInfrastructure.Context;
using TaskStatus = Onboard_management_system.OnboardingDomain.Enums.TaskStatus;

namespace Onboard_management_system.OnboardingApplication.Services;

public class OnboardingTaskService : IOnboardingTaskService
{
    private readonly OnboardingDbContext _context;
    private readonly IMapper _mapper;

    public OnboardingTaskService(OnboardingDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OnboardingTaskDto>> GetAllAsync()
    {
        var tasks = await _context.OnboardingTasks
            .Include(t => t.ResponsibleDepartment)
            .Include(t => t.ResponsibleUser)
            .ToListAsync();

        return _mapper.Map<IEnumerable<OnboardingTaskDto>>(tasks);
    }

    public async Task<OnboardingTaskDto?> GetByIdAsync(int id)
    {
        var task = await _context.OnboardingTasks
            .Include(t => t.ResponsibleDepartment)
            .Include(t => t.ResponsibleUser)
            .FirstOrDefaultAsync(t => t.Id == id);

        return task is null ? null : _mapper.Map<OnboardingTaskDto>(task);
    }

    public async Task<bool> UpdateStatusAsync(int id, UpdateOnboardingTaskStatusDto dto)
    {
        var task = await _context.OnboardingTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null) return false;

        // İş kuralı: tamamlanan bir görev tekrar bekliyor durumuna alınamaz
        if (task.Status == TaskStatus.Completed && dto.NewStatus == TaskStatus.Pending)
            throw new InvalidOperationException("Tamamlanmış bir görev tekrar 'Bekliyor' durumuna alınamaz.");

        var oldStatus = task.Status;
        task.Status = dto.NewStatus;

        if (dto.NewStatus == TaskStatus.Completed)
        {
            task.CompletedAt = DateTime.UtcNow;
            task.CompletedByUserId = dto.ChangedByUserId;
        }

        // İş kuralı: her durum değişikliği işlem geçmişine kaydedilmeli
        _context.TaskStatusHistories.Add(new TaskStatusHistory
        {
            OnboardingTaskId = task.Id,
            OldStatus = oldStatus,
            NewStatus = dto.NewStatus,
            ChangedByUserId = dto.ChangedByUserId,
            ChangedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        await TryCompleteOnboardingProcessAsync(task.OnboardingProcessId);

        return true;
    }

    public async Task<bool> UpdateNoteAsync(int id, UpdateOnboardingTaskNoteDto dto)
    {
        var task = await _context.OnboardingTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null) return false;

        task.Note = dto.Note;
        await _context.SaveChangesAsync();
        return true;
    }

    // İş kuralı: zorunlu görevler tamamlanmadan onboarding süreci tamamlanamaz;
    // hepsi bitince süreç otomatik "Completed" olur.
    private async Task TryCompleteOnboardingProcessAsync(int onboardingProcessId)
    {
        var mandatoryTasks = await _context.OnboardingTasks
            .Where(t => t.OnboardingProcessId == onboardingProcessId && t.IsMandatory)
            .ToListAsync();

        if (mandatoryTasks.Count == 0) return;

        var allCompleted = mandatoryTasks.All(t => t.Status == TaskStatus.Completed);
        if (!allCompleted) return;

        var process = await _context.OnboardingProcesses.FirstOrDefaultAsync(p => p.Id == onboardingProcessId);
        if (process is null || process.Status == OnboardingStatus.Completed) return;

        process.Status = OnboardingStatus.Completed;
        process.EndDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<TaskStatusHistoryDto>> GetHistoryAsync(int taskId)
    {
        var history = await _context.TaskStatusHistories
            .Include(h => h.ChangedByUser)
            .Where(h => h.OnboardingTaskId == taskId)
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TaskStatusHistoryDto>>(history);
    }
}