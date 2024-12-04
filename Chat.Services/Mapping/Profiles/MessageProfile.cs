using AutoMapper;
using Chat.Data.Entities;
using Chat.Services.Mapping.DTO_s;

namespace Chat.Services.Mapping.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile():base()
        {
            CreateMap<Message , MessageDto>().ReverseMap();
        }
    }
}
