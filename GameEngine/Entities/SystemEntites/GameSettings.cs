using GameEngine.Entities.GameEntities;

using System.ComponentModel.DataAnnotations;

namespace GameEngine.Entities.SystemEntites
{
    public class GameSettings
    {
        [Range(2, 6)]
        public int PlayersCount { get; set; }
        public int PlayersStartCardsCount { get; set; }
        public DeckType DeckType { get; set; }
    }
}