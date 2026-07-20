using Microsoft.EntityFrameworkCore;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingInfrastructure.Context;

public class OnboardingDbContext : DbContext
{
    public OnboardingDbContext(DbContextOptions<OnboardingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<User> Users => Set<User>();
    public DbSet<OnboardingTemplate> OnboardingTemplates => Set<OnboardingTemplate>();
    public DbSet<OnboardingTemplateTask> OnboardingTemplateTasks => Set<OnboardingTemplateTask>();
    public DbSet<OnboardingProcess> OnboardingProcesses => Set<OnboardingProcess>();
    public DbSet<OnboardingTask> OnboardingTasks => Set<OnboardingTask>();
    public DbSet<TaskStatusHistory> TaskStatusHistories => Set<TaskStatusHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OnboardingDbContext).Assembly);
    }
}