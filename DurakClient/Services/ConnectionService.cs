using Connections.Services;

using Grpc.Net.Client;

using System;
using System.Threading.Tasks;

namespace DurakClient.Services
{
    public class ConnectionService
    {
        public async Task<Guid> Connect(string nickname)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5058");
            var client = new Connections.Services.ConnectionService.ConnectionServiceClient(channel);
            var request = new LoginRequest() { NickName = nickname };
            var responce = await client.ConnectAsync(request);
            return Guid.Parse(responce.Id);
        }
    }
}
