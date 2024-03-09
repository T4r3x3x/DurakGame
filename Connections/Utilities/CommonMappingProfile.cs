using AutoMapper;

using Connections.Services;

using GameEngine.Entities.GameEntities;
using GameEngine.Entities.SystemEntites;



namespace Common.Utilities
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {
            CreateMap<GameSettings, LobbySetting>().ReverseMap();
            CreateMap<CardMessage, Card>().ReverseMap();
        }
    }
}
