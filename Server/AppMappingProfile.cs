﻿using AutoMapper;

using Connections.Services;

using GameEngine.Entities.SystemEntites;

using Server.Entities;


namespace Server
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<GameSettings, LobbySetting>();
            CreateMap<LobbySetting, GameSettings>();
            CreateMap<User, PlayerModel>();
            CreateMap<PlayerModel, User>();
            CreateMap<Lobby, LobbyModel>();
            CreateMap<LobbyModel, Lobby>();
            CreateMap<Connections.Services.Card, GameEngine.Entities.GameEntities.Card>();
            CreateMap<GameEngine.Entities.GameEntities.Card, Connections.Services.Card>();
        }
    }
}
