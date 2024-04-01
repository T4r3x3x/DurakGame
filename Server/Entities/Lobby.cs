using GameEngine.Entities.SystemEntites;
using Server.CustomCollections;
using Server.Services;

namespace Server.Entities
{
    public class Lobby
    {
        public required Guid Guid { get; init; }
        public required string Name { get; init; }
        public string? Password { get; init; }
        public required GameSettings Settings { get; init; }
        public required User Owner { get; init; }
        public required PlayersList Players { get; init; }
        public Game? Game { get; set; }
        public bool IsFull => Players.Count == Settings.PlayersCount;
    }
}