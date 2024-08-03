﻿using AutoMapper;
using user_management.core.Commands.Onboarding;
using user_management.core.DataTransferObjects;
using user_management.domain.Entities;


namespace user_management.core.Profiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<HandleSignUp.Command, AppUser>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}