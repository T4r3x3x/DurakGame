using AutoMapper;

using Connections.Services;

using Server.Entities;

namespace Server.Utilities
{
    public class ServerMappingProfile : Profile
    {
        public ServerMappingProfile()
        {
            CreateMap<User, PlayerModel>().ReverseMap();
            CreateMap<Lobby, LobbyModel>().ReverseMap();
        }
    }
}