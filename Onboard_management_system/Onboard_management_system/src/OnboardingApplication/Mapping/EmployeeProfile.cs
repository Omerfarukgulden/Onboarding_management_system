using AutoMapper;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingApplication.Mapping;

public class EmployeeProfile : Profile
{
   public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.EmpName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.EmpSurname, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.EmpEmail, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.EmpPhone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.EmpAddress, opt => opt.MapFrom(src => src.EmpAddress))
            .ForMember(dest => dest.EmpBlood, opt => opt.MapFrom(src => src.EmpBlood))
            .ForMember(dest => dest.EmpGender, opt => opt.MapFrom(src => src.EmpGender))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.Name));
 
        CreateMap<CreateEmployeeDto, Employee>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.EmpName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.EmpSurname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmpEmail))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.EmpPhone))
            .ForMember(dest => dest.EmpAddress, opt => opt.MapFrom(src => src.EmpAddress))
            .ForMember(dest => dest.EmpBlood, opt => opt.MapFrom(src => src.EmpBlood))
            .ForMember(dest => dest.EmpGender, opt => opt.MapFrom(src => src.EmpGender));
 
        CreateMap<UpdateEmployeeDto, Employee>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.EmpName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.EmpSurname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmpEmail))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.EmpPhone))
            .ForMember(dest => dest.EmpAddress, opt => opt.MapFrom(src => src.EmpAddress))
            .ForMember(dest => dest.EmpBlood, opt => opt.MapFrom(src => src.EmpBlood))
            .ForMember(dest => dest.EmpGender, opt => opt.MapFrom(src => src.EmpGender));
    }
}