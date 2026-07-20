using AutoMapper;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingApplication.Mapping;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.Name));

        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>();
    }
}