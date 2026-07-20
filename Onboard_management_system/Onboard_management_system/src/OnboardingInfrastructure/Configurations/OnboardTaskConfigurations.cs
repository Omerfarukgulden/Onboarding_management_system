using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingInfrastructure.Configurations;

public class OnboardingTaskConfigurations : IEntityTypeConfiguration<OnboardingTask>
{
    public void Configure(EntityTypeBuilder<OnboardingTask> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Description).HasMaxLength(500);
        builder.Property(t => t.DueDate).IsRequired();
        builder.Property(t => t.Status).IsRequired().HasConversion<string>();
        builder.Property(t => t.Note).HasMaxLength(1000);

        builder.HasOne(t => t.ResponsibleDepartment)
            .WithMany()
            .HasForeignKey(t => t.ResponsibleDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ResponsibleUser)
            .WithMany()
            .HasForeignKey(t => t.ResponsibleUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.CompletedByUser)
            .WithMany()
            .HasForeignKey(t => t.CompletedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.StatusHistory)
            .WithOne(h => h.OnboardingTask)
            .HasForeignKey(h => h.OnboardingTaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}