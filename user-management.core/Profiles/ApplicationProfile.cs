using AutoMapper;
using Microsoft.AspNetCore.Identity;
using user_management.core.Commands.Onboarding;
using user_management.core.Commands.Tenant;
using user_management.core.DataTransferObjects;
using user_management.core.Queries.Role;
using user_management.core.Queries.Tenant;
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
            CreateMap<HandleFetchRoleById.Result, IdentityRole>().ReverseMap();
            CreateMap<HandleFetchRoles.Result, IdentityRole>().ReverseMap();
            CreateMap<HandleFetchTenantById.Result, Tenant>().ReverseMap();
        }
    }
}