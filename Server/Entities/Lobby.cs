using GameEngine.Entities.SystemEntites;

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
        public required List<User> Players { get; init; }
        public Game? Game { get; set; }
    }
}