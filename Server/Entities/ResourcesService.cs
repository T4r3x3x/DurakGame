

namespace Server.Entities
{
    public class ResourcesService
    {
        public Dictionary<int, Player> Players { get; set; } = new Dictionary<int, Player>();
        public Dictionary<int, Lobby> Lobbies { get; set; } = new Dictionary<int, Lobby>();

        private int _playerId = 0;
        private int _lobbyId = 0;

        public int AddPlayer(Player player)
        {
            Players.Add(player.Id, player);
            return _playerId;
        }

    }
}
