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
// tüm taskları getiren method 
    public async Task<IEnumerable<OnboardingTaskDto>> GetAllAsync()
    {
        var tasks = await _context.OnboardingTasks
            .Include(t => t.ResponsibleDepartment)
            .Include(t => t.ResponsibleUser)
            .ToListAsync();

        return _mapper.Map<IEnumerable<OnboardingTaskDto>>(tasks);
    }
//taskları çalışan idsine göre getiren method 
    public async Task<OnboardingTaskDto?> GetByIdAsync(int id)
    {
        var task = await _context.OnboardingTasks
            .Include(t => t.ResponsibleDepartment)
            .Include(t => t.ResponsibleUser)
            .FirstOrDefaultAsync(t => t.Id == id);

        return task is null ? null : _mapper.Map<OnboardingTaskDto>(task);
    }
// taskların durumunu idsine göre güncelleyen method 
    public async Task<bool> UpdateStatusAsync(int id, UpdateOnboardingTaskStatusDto dto)
    {
        var task = await _context.OnboardingTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null) return false;

        // eğer bir görev bir kere tamamlandıysa tekrar diğer durumlara geçirilemez 
        if (task.Status == TaskStatus.Completed && dto.NewStatus == TaskStatus.Pending)
            throw new InvalidOperationException("Tamamlanmış bir görev tekrar 'Bekliyor' durumuna alınamaz.");

        var oldStatus = task.Status;
        task.Status = dto.NewStatus;

        if (dto.NewStatus == TaskStatus.Completed)
        {
            task.CompletedAt = DateTime.UtcNow;
            task.CompletedByUserId = dto.ChangedByUserId;
        }

        // iş durumu hakkında güncelleme yapıldıgında anlık zamanı kaydeder
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

    // taska not eklemeye yarayan method 
    public async Task<bool> UpdateNoteAsync(int id, UpdateOnboardingTaskNoteDto dto)
    {
        var task = await _context.OnboardingTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null) return false;

        task.Note = dto.Note;
        await _context.SaveChangesAsync();
        return true;
    }

    //zorunlu olarak tamamlanması beklenen görevler bitmeden görevin bittiği varsılayamazı sağlayan method 
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
    
    //taskda yapılan değişiklerin tarihe göre açıklamasını yapan method 
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