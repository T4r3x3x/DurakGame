using GameEngine.Entities.SystemEntites;

using System;

namespace DurakClient.MVVM.Models
{
    public class LobbyCreateModel
    {
        public required Guid CreatorId { get; init; }
        public required string Name { get; init; }
        public string? Password { get; init; }
        public required GameSettings GameSettings { get; init; }
    }
}