using AutoMapper;

using Connections.Services;

using Server.Entities;

namespace Server.Utilities
{
    public class ServerMappingProfile : Profile
    {
        public ServerMappingProfile()
        {
            CreateMap<User, PlayerModel>();
            CreateMap<PlayerModel, User>();
            CreateMap<Lobby, LobbyModel>();
            CreateMap<LobbyModel, Lobby>();
        }
    }
}