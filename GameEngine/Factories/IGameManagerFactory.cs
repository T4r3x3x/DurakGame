using GameEngine.Entities.GameEntities;
using GameEngine.Entities.SystemEntites;
using GameEngine.Shufflers;

namespace GameEngine.Factories
{
    internal interface IGameManagerFactory
    {
        Game GetGameManager(GameSettings gameSettings, IShuffler<Card> shuffler);
    }
}
