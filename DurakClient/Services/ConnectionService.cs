using Connections.Services;

using System;
using System.Threading.Tasks;

using static Connections.Services.ConnectionService;

namespace DurakClient.Services
{
    public class ConnectionService
    {
        private readonly ConnectionServiceClient _clientService;

        public ConnectionService(ConnectionServiceClient clientService) => _clientService = clientService;

        public async Task<Guid> Connect(string nickname)
        {
            var request = new LoginRequest() { NickName = nickname };
            var responce = await _clientService.ConnectAsync(request);
            return Guid.Parse(responce.Id);
        }

        public async Task Disconnect(Guid guid)
        {
            var request = new DisconnectRequest() { Id = guid.ToString() };
            await _clientService.DisconnectAsync(request);
        }
    }
}