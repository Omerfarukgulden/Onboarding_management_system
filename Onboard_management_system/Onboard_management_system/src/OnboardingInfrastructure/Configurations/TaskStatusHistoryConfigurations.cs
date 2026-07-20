using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingInfrastructure.Configurations;

public class TaskStatusHistoryConfigurations : IEntityTypeConfiguration<TaskStatusHistory>
{
    public void Configure(EntityTypeBuilder<TaskStatusHistory> builder)
    {
        builder.HasKey(h => h.Id);
        builder.Property(h => h.OldStatus).IsRequired().HasConversion<string>();
        builder.Property(h => h.NewStatus).IsRequired().HasConversion<string>();
        builder.Property(h => h.ChangedAt).IsRequired();

        builder.HasOne(h => h.ChangedByUser)
            .WithMany()
            .HasForeignKey(h => h.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}