using AutoMapper;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingApplication.Mapping;

public class TaskStatusHistoryProfile : Profile
{
    public TaskStatusHistoryProfile()
    {
        CreateMap<TaskStatusHistory, TaskStatusHistoryDto>()
            .ForMember(dest => dest.ChangedByUsername, opt => opt.MapFrom(src => src.ChangedByUser.Username));
    }
}