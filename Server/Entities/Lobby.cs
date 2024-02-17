using GameEngine.Entities.SystemEntites;

using System.Collections.ObjectModel;

namespace Server.Entities
{
    public class Lobby
    {
        public required Guid Guid { get; init; }
        public required string Name { get; init; }
        public string? Password { get; init; }
        public required GameSettings Settings { get; init; }
        public required User Owner { get; init; }
        public required ObservableCollection<User> Players { get; init; }
        public Game? Game { get; set; }
    }
}