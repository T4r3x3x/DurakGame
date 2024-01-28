using GameEngine.Entities;

using System.ComponentModel.DataAnnotations;

namespace GameEngine
{
    public class GameSettings
    {
        [Range(2, 6)]
        public int PlayersCount { get; set; }
        public int PlayersStartCardsCount { get; set; }
        public DeckType DeckType { get; set; }
    }
}