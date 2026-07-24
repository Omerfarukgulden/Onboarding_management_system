using Microsoft.EntityFrameworkCore;
using Onboard_management_system.OnboardingDomain.Entities;
using Onboard_management_system.OnboardingDomain.Enums;
using Onboard_management_system.OnboardingInfrastructure.Context;


namespace Onboard_management_system.OnboardingInfrastructure.Seed;

public static class SeedData
{
    public static async Task SeedAsync(OnboardingDbContext context)
    {
        await context.Database.MigrateAsync();
        
        await SeedDepartmentsAsync(context);
        await SeedPositionsAsync(context);
        await SeedUsersAsync(context);
        await SeedOnboardingTemplatesAsync(context);
        
    }

    private static async Task SeedDepartmentsAsync(OnboardingDbContext context)
    {
        if (await context.Departments.AnyAsync()) return;

        var departments = new List<Department>
        {
            new() { Name = "İnsan Kaynakları", Description = "İK departmanı", IsActive = true },
            new() { Name = "Yazılım Geliştirme", Description = "Yazılım departmanı", IsActive = true },
            new() { Name = "Finans", Description = "Finans departmanı", IsActive = true },
            new() { Name = "Pazarlama", Description = "Pazarlama departmanı", IsActive = true },
        };
        
        await context.Departments.AddRangeAsync(departments);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPositionsAsync(OnboardingDbContext context)
    {
        if (await context.Positions.AnyAsync()) return;

        var positions = new List<Position>
        {
            new() { Name = "Yazılım Geliştirici", Description = "Backend/Frontend geliştirici" },
            new() { Name = "İK Uzmanı", Description = "İnsan kaynakları uzmanı" },
            new() { Name = "Finans Analisti", Description = "Finansal analiz uzmanı" },
            new() { Name = "Pazarlama Uzmanı", Description = "Dijital pazarlama uzmanı" },
        };
        await context.Positions.AddRangeAsync(positions);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(OnboardingDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var users = new List<User>
        {   new() { Username = "admin", Email = "admin@onboarding.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"), Role = UserRole.Admin, IsActive = true, CreatedAt = DateTime.UtcNow }, 
            new() { Username = "ik.sorumlu", Email = "ik@onboarding.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Ik1234!"), Role = UserRole.Ik, DepartmentId = 1, IsActive = true,CreatedAt = DateTime.UtcNow},
            new() { Username = "yazilim.sorumlu", Email = "yazilim@onboarding.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Dept123!"), Role = UserRole.DepartmentUser, DepartmentId = 2, IsActive = true, CreatedAt = DateTime.UtcNow }
        };
        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }

    private static async Task SeedOnboardingTemplatesAsync(OnboardingDbContext context)
    {
        if (await context.OnboardingTemplates.AnyAsync()) return;

        var template = new OnboardingTemplate
        {
            Name = "Yazılım Personeli Onboarding Şablonu",
            Description = "Yeni yazılım geliştiriciler için standart onboarding şablonu",
            IsActive = true,
            TemplateTasks = new List<OnboardingTemplateTask>
            {
                new()
                {
                    Title = "Kurumsal e-posta hesabının açılması",
                    Description = "Yeni çalışan için şirket email hesabı oluşturulacak.",
                    ResponsibleDepartmentId = 1, 
                    IsMandatory = true,
                    DueInDays = 1
                },
                new()
                {
                    Title = "Bilgisayar ve ekipman hazırlanması",
                    Description = "Çalışanın kullanacağı bilgisayar ve ekipmanların hazırlanması.",
                    ResponsibleDepartmentId = 2, 
                    IsMandatory = true,
                    DueInDays = 1
                },
                new()
                {
                    Title = "Personel kartının oluşturulması",
                    Description = "Çalışan kimlik kartı ve giriş yetkisi oluşturulacak.",
                    ResponsibleDepartmentId = 1,
                    IsMandatory = true,
                    DueInDays = 3
                },
                new()
                {
                    Title = "Gerekli uygulama yetkilerinin tanımlanması",
                    Description = "Jira, Confluence, Git gibi geliştirici araçlarına erişim.",
                    ResponsibleDepartmentId = 2, 
                    IsMandatory = true,
                    DueInDays = 3
                },
                new()
                {
                    Title = "Oryantasyon toplantısının planlanması",
                    Description = "Yeni çalışanla oryantasyon toplantısı organize edilecek.",
                    ResponsibleDepartmentId = 1, 
                    IsMandatory = false,
                    DueInDays = 7
                }
            }
        };

        await context.OnboardingTemplates.AddAsync(template);
        await context.SaveChangesAsync();
    }
}