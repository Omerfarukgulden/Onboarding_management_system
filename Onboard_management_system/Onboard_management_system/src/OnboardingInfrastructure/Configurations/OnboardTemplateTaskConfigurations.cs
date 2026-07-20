using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingInfrastructure.Configurations;

public class OnboardingTemplateTaskConfigurations : IEntityTypeConfiguration<OnboardingTemplateTask>
{
    public void Configure(EntityTypeBuilder<OnboardingTemplateTask> builder)
    {
        builder.HasKey(tt => tt.Id);
        builder.Property(tt => tt.Title).IsRequired().HasMaxLength(200);
        builder.Property(tt => tt.Description).HasMaxLength(500);
        builder.Property(tt => tt.DueInDays).IsRequired();

        builder.HasOne(tt => tt.ResponsibleDepartment)
            .WithMany()
            .HasForeignKey(tt => tt.ResponsibleDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}