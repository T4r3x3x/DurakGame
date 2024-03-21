using AutoMapper;

using Connections.Services;

using Server.Entities;
using Server.Services;

namespace Server.Utilities
{
    public class ServerMappingProfile : Profile
    {
        public ServerMappingProfile()
        {
            CreateMap<User, PlayerModel>().ReverseMap();
            CreateMap<Lobby, LobbyResponce>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Guid))
                .ForMember(x => x.HasPassword, opt => opt.MapFrom(x => x.Password != null))
                .ForMember(x => x.JoindePlayersCount, opt => opt.MapFrom(x => x.Players.Count));
        }
    }
}