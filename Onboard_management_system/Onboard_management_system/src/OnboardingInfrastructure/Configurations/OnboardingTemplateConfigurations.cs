using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingInfrastructure.Configurations;

public class OnboardingTemplateConfigurations : IEntityTypeConfiguration<OnboardingTemplate>
{
    public void Configure(EntityTypeBuilder<OnboardingTemplate> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(250);

        builder.HasMany(t => t.TemplateTasks)
            .WithOne(tt => tt.OnboardingTemplate)
            .HasForeignKey(tt => tt.OnboardingTemplateId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}