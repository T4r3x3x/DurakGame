using Connections.Services;

using Google.Protobuf.WellKnownTypes;

using System;
using System.Threading.Tasks;

using static Connections.Services.LobbyService;

namespace DurakClient.Services
{
    public class LobbyService
    {
        private readonly LobbyServiceClient _clientService;
        private readonly Guid _guid;

        private static readonly Empty s_empty = new Empty();

        public LobbyService(LobbyServiceClient clientService, Guid guid)
        {
            _clientService = clientService;
            _guid = guid;
        }

        public async Task<LobbyList> GetAllLobby()
        {
            var responce = await _clientService.GetAllLobiesAsync(s_empty);
            //map to inner model
        }
    }
}
