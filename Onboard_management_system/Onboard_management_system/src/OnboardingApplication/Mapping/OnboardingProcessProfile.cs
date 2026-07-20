using AutoMapper;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingApplication.Mapping;

public class OnboardingProcessProfile : Profile
{
    public OnboardingProcessProfile()
    {
        CreateMap<OnboardingProcess, OnboardingProcessDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
            .ForMember(dest => dest.OnboardingTemplateName, opt => opt.MapFrom(src => src.OnboardingTemplate.Name));
    }
}