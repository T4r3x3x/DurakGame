

namespace Server.Entities
{
    public static class Resources
    {
        public static Dictionary<Guid, Player> Players { get; set; } = new();
        public static Dictionary<Guid, Lobby> Lobbies { get; set; } = new();
    }
}
