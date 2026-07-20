using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingInfrastructure.Configurations;

public class OnboardingProcessConfigurations : IEntityTypeConfiguration<OnboardingProcess>
{
    public void Configure(EntityTypeBuilder<OnboardingProcess> builder)
    {
        builder.HasKey(op => op.Id);
        
        builder.HasOne(op => op.Employee)
            .WithMany(e =>e.OnboardingProcesses)
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(op => op.StartDate)
            .IsRequired();
        builder.Property(op => op.EndDate)
            .IsRequired(false);
        
        builder.Property(op => op.Status)
            .IsRequired()
            .HasConversion<string>();
    }
}
