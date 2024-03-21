using AutoMapper;

using Connections.Services;

using DurakClient.MVVM.Models;

namespace DurakClient.Setup.Mapping
{
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<LobbyResponce, Lobby>();
        }
    }
}
