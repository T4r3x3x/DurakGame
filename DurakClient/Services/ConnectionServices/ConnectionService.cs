using Connections.Services;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

using static Connections.Services.ConnectionService;

namespace DurakClient.Services.ConnectionServices
{
    public class ConnectionService : IConnectionService
    {
        private readonly ConnectionServiceClient _connectionService;

        public ConnectionService(ConnectionServiceClient clientService) => _connectionService = clientService;

        public async Task<Guid> Connect(string nickname)
        {
            var request = new LoginRequest() { NickName = nickname };
            ConnectionReply responce = null;
            try
            {
                responce = await _connectionService.ConnectAsync(request);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return Guid.Parse(responce.Id);


        }

        public async Task Disconnect(Guid? guid)
        {
            if (guid is null)
                return;
            var request = new DisconnectRequest() { Id = guid.ToString() };
            await _connectionService.DisconnectAsync(request);
        }
    }
}