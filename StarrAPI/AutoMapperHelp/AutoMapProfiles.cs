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
            CreateMap<AppUserDTO,AppUser>();
            CreateMap<Messages,MessagesDTO>()
            .ForMember(d => d.SenderPhotoUrl,o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.MainPic).PhotoUrl))
            .ForMember(d => d.RecipientPhotoUrl,o => o.MapFrom(s => s.Recipient.Photos.FirstOrDefault(x => x.MainPic).PhotoUrl));
        }
    }
}