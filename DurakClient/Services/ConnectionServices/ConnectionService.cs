﻿using Connections.Services;

using Grpc.Net.Client;

using System;
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

            var channel = GrpcChannel.ForAddress("http://localhost:5058");
            var client = new Connections.Services.ConnectionService.ConnectionServiceClient(channel);
            var request = new LoginRequest() { NickName = nickname };
            var responce = await client.ConnectAsync(request);
            return Guid.Parse(responce.Id);

        }

        public async Task Disconnect(Guid guid)
        {
            var request = new DisconnectRequest() { Id = guid.ToString() };
            await _connectionService.DisconnectAsync(request);
        }
    }
}