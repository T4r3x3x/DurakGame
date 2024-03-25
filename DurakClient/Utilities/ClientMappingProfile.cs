using AutoMapper;

using Connections.Services;

using DurakClient.MVVM.Models;

using GameEngine.Entities.GameEntities;

namespace DurakClient.Utilities
{
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<LobbyResponce, Lobby>()
                .ForMember(x => x.Guid, opt => opt.MapFrom(x => x.Id));
            CreateMap<Card, CardMessage>().ReverseMap();
            CreateMap<CreateLobbyModel, CreateLobbyRequest>().ReverseMap();
        }
    }
}
