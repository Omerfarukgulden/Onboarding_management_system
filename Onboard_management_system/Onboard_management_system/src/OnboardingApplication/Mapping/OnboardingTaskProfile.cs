using AutoMapper;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingApplication.Mapping;

public class OnboardingTaskProfile : Profile
{
    public OnboardingTaskProfile()
    {
        CreateMap<OnboardingTask, OnboardingTaskDto>()
            .ForMember(dest => dest.ResponsibleDepartmentName,
                opt => opt.MapFrom(src => src.ResponsibleDepartment != null ? src.ResponsibleDepartment.Name : null))
            .ForMember(dest => dest.ResponsibleUserName,
                opt => opt.MapFrom(src => src.ResponsibleUser != null ? src.ResponsibleUser.Username : null));
    }
}