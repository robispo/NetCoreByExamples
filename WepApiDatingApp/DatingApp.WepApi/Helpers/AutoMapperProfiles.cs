using AutoMapper;
using DatingApp.WepApi.Dtos;
using DatingApp.WepApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.WepApi.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); })
                .ForMember(dest => dest.Age, opt => { opt.ResolveUsing(d=>d.DateOfBirth.GetAge()); });

            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); })
                .ForMember(dest => dest.Age, opt => { opt.ResolveUsing(d => d.DateOfBirth.GetAge()); });

            CreateMap<Photo, PhotosForDetailedDto>();
        }
    }
}
