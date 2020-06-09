using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MozzieAiSystems.Dtos;
using MozzieAiSystems.Models;
using Profile = AutoMapper.Profile;

namespace MozzieAiSystems.Configs
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateLocationRequest, Location>();
            CreateMap<LocationFileInput, LocationFile>();
            CreateMap<Location, LocalActivityListResponse>();
        }
    }
}
