using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingInfrastructure.Configurations;

public class PositionConfigurations : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Description).HasMaxLength(200);

        builder.HasMany(p => p.Employees)
            .WithOne(e => e.Position)
            .HasForeignKey(e => e.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}