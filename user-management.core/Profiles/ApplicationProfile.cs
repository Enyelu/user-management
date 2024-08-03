using AutoMapper;
using user_management.core.Commands.Onboarding;
using user_management.core.Commands.Tenant;
using user_management.core.DataTransferObjects;
using user_management.core.Queries.User;
using user_management.domain.Entities;


namespace user_management.core.Profiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<HandleSignUp.Command, AppUser>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<HandleFetchUserBySearchParameter.Result, AppUser>().ReverseMap();
            CreateMap<HandleResetPassword.Command, PasswordRestDto>().ReverseMap();
            CreateMap<HandleCreateTenant.Command, Tenant>().ReverseMap();
        }
    }
}