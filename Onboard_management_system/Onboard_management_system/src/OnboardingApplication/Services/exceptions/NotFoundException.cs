namespace Onboard_management_system.OnboardingApplication.Services.exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}