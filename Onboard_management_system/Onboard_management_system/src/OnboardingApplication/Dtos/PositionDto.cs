namespace Onboard_management_system.OnboardingApplication.Dtos;

public class PositionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class CreatePositionDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdatePositionDto : CreatePositionDto
{
}