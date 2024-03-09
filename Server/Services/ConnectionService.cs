using Connections.Services;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Server.Entities;

namespace Server.Services
{
    public class ConnectionService : Connections.Services.ConnectionService.ConnectionServiceBase
    {
        private readonly ConnectionResources _resources;
        private readonly ILogger<ConnectionService> _logger;

        private static readonly Empty s_empty = new Empty();

        public ConnectionService(ILogger<ConnectionService> logger, ConnectionResources resources)
        {
            _logger = logger;
            _resources = resources;
        }

        public override Task<ConnectionReply> Connect(LoginRequest request, ServerCallContext context)
        {
            Guid userId = Guid.NewGuid();
            User user = new User { Guid = userId, NickName = request.NickName };
            _resources.Users.TryAdd(userId, user);

            _logger.LogInformation($"User {user.NickName} with id: {user.Guid} has been connected!");

            return Task.FromResult(new ConnectionReply { Id = userId.ToString() });
        }

        public override Task<Empty> Disconnect(DisconnectRequest request, ServerCallContext context)
        {
            var user = _resources.GetUser(request.Id);
            _resources.Users.Remove(Guid.Parse(request.Id), out var deletedUser);

            _logger.LogInformation($"User {user.NickName} with id: {user.Guid} has been disconnected!");

            return Task.FromResult(s_empty);
        }
    }
}
