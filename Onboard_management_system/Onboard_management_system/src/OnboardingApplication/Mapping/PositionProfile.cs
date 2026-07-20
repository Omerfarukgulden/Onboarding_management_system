using AutoMapper;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingApplication.Mapping;

public class PositionProfile : Profile
{
    public PositionProfile()
    {
        CreateMap<Position, PositionDto>();
        CreateMap<CreatePositionDto, Position>();
        CreateMap<UpdatePositionDto, Position>();
    }
}