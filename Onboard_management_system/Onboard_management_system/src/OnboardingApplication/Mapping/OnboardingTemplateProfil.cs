using AutoMapper;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingApplication.Mapping;

public class OnboardingTemplateProfile : Profile
{
    public OnboardingTemplateProfile()
    {
        CreateMap<OnboardingTemplate, OnboardingTemplateDto>();
        CreateMap<CreateOnboardingTemplateDto, OnboardingTemplate>();
        CreateMap<UpdateOnboardingTemplateDto, OnboardingTemplate>();

        CreateMap<OnboardingTemplateTask, OnboardingTemplateTaskDto>()
            .ForMember(dest => dest.ResponsibleDepartmentName,
                opt => opt.MapFrom(src => src.ResponsibleDepartment != null ? src.ResponsibleDepartment.Name : null));

        CreateMap<CreateOnboardingTemplateTaskDto, OnboardingTemplateTask>();
    }
}