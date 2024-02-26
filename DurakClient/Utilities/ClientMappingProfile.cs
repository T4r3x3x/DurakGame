using AutoMapper;

using Connections.Services;

using DurakClient.MVVM.Models;

namespace DurakClient.Utilities
{
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<LobbyModel, Lobby>();
        }
    }
}
