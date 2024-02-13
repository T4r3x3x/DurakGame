using GameEngine.Entities.SystemEntites;

namespace Server.Entities
{
    public class Lobby
    {
        public Guid Guid { get; set; }
        public required string Name { get; set; }
        public string? Password { get; set; }
        public required GameSettings Settings { get; set; }
        public required List<Player> Players { get; set; }
    }
}
