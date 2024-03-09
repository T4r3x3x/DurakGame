using GameEngine.Entities.SystemEntites;

namespace DurakClient.MVVM.Models
{
    public class Lobby
    {
        public required string Name { get; set; }
        public required GameSettings Settings { get; init; }
        public bool HasPassword { get; init; }
        public int JoinedPlayersCount { get; init; }
    }
}
