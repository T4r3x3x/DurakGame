﻿using AutoMapper;

using Connections.Services;

using DurakClient.MVVM.Models;

using GameEngine.Entities.GameEntities;

namespace DurakClient.Utilities
{
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<Lobby, LobbyResponce>().ReverseMap();
            CreateMap<Card, CardMessage>().ReverseMap();
        }
    }
}
