using AutoMapper;
using HotelTask.API.DTOs;
using HotelTask.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelTask.API.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UpdateRoomDto, Room>().ReverseMap();
        }
    }
}
