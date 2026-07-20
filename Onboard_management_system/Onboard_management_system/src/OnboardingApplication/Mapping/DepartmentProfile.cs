using AutoMapper;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingApplication.Mapping;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDto>();
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();
    }
}