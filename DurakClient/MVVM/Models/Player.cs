using System;

namespace DurakClient.MVVM.Models
{
    public class Player
    {
        public required Guid Guid { get; init; }
        public required string Nickname { get; init; }
        public bool AreReady { get; set; }
    }
}
