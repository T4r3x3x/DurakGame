using GameEngine.Entities.SystemEntites;

using System;

namespace DurakClient.MVVM.Models
{
    public class Lobby
    {
        public required Guid Guid { get; init; }
        public required string Name { get; set; }
        public required GameSettings Settings { get; init; }
        public required bool HasPassword { get; init; }
        public required int JoinedPlayersCount { get; init; }
    }
}
