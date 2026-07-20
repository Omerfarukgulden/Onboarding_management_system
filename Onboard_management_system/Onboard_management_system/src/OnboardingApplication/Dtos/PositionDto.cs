namespace Onboard_management_system.OnboardingApplication.Dtos;


//posizyon için gerekli veriler 
public class PositionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

//yeni pozisyon oluşturmak için gerekli veriler 
public class CreatePositionDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}


//pozisyonu güncellemek için gereken veriler 
public class UpdatePositionDto : CreatePositionDto
{
}