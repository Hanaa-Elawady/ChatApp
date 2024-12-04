using AutoMapper;
using Chat.Data.Entities;
using Chat.Services.Mapping.DTO_s;

namespace Chat.Services.Mapping.Profiles
{
    public class ConnectionProfile : Profile
    {
        public ConnectionProfile() : base()
        {
            CreateMap<UserConnection, ConnectionDto>()
                .ForMember(des => des.ConnectionId, options => options.MapFrom(src => src.Id))
                .ForMember(des => des.PhoneNumber, options => options.MapFrom(src => src.User2.PhoneNumber))
                .ForMember(des => des.PersonId, options => options.MapFrom(src => src.User2.Id))
                .ForMember(des => des.DisplayName, options => options.MapFrom(src => src.User2.DisplayName))
                .ForMember(des => des.Messages, options => options.MapFrom(src => src.Messages))
                .ForMember(des => des.ProfilePicture, options => options.MapFrom(src => src.User2.ProfilePicture)).ReverseMap();
            
        }
    }
}
