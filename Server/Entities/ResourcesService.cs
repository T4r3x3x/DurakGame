

namespace Server.Entities
{
    public class ResourcesService
    {
        public Dictionary<Guid, Player> Players { get; set; } = new();
        public Dictionary<Guid, Lobby> Lobbies { get; set; } = new();
    }
}
