using System;

namespace DurakClient.Services
{
    public class Resources
    {
        public Guid PlayerId { get; set; } = Guid.Empty;
        public Guid LobbyId { get; set; } = Guid.Empty;
    }
}