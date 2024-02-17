using GameEngine.Entities.SystemEntites;

namespace Server.Entities
{
    public class Lobby
    {
        public required Guid Guid { get; set; }
        public required string Name { get; set; }
        public string? Password { get; set; }
        public required GameSettings Settings { get; set; }
        public required User Owner { get; set; }
        public required List<User> Players { get; set; }
        public Game? Game { get; set; }
    }
}