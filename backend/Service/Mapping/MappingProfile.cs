using AutoMapper;
using Data.Entities;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<CreateUserDTO, User>();
            CreateMap<UpdateUserDTO, User>();

            // DonationLink
            CreateMap<DonationLink, DonationLinkDTO>().ReverseMap();
            CreateMap<CreateDonationLinkDTO, DonationLink>();
            CreateMap<UpdateDonationLinkDTO, DonationLink>();
        }
    }
}
