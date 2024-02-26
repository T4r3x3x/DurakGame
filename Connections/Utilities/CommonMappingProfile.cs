using AutoMapper;

using Connections.Services;

using GameEngine.Entities.SystemEntites;



namespace Common.Utilities
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {
            CreateMap<GameSettings, LobbySetting>();
            CreateMap<LobbySetting, GameSettings>();
            CreateMap<Card, GameEngine.Entities.GameEntities.Card>();
            CreateMap<GameEngine.Entities.GameEntities.Card, Card>();
        }
    }
}
