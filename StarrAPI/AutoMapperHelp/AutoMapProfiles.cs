using System.Linq;
using AutoMapper;
using StarrAPI.DTOs;
using StarrAPI.Extensions;
using StarrAPI.Models;

namespace StarrAPI.AutoMapperHelp
{
    public class AutoMapProfiles:Profile
    {
        public AutoMapProfiles()
        {
            CreateMap<AppUser,MemberDTO>().ForMember(d => d.PhotoUrl,o => o.MapFrom(s =>s.Photos.FirstOrDefault(x => x.MainPic).PhotoUrl))
                                          .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalulateAge()));
            CreateMap<Photos,PhotosDTO>();
            CreateMap<MemberUpdateDTO,AppUser>();
        }
    }
}