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

        public override Task<PlayerId> Connect(LoginRequest request, ServerCallContext context)
        {
            Guid userId = Guid.NewGuid();
            User user = new User { Guid = userId, NickName = request.NickName };
            _resources.Users.Add(userId, user);

            _logger.LogInformation($"User {user.NickName} with id: {user.Guid} has been connected!");

            return Task.FromResult(new PlayerId { Id = userId.ToString() });
        }

        public override Task<Empty> Disconnect(PlayerId request, ServerCallContext context)
        {
            var user = _resources.GetUser(request.Id);
            _resources.Users.Remove(Guid.Parse(request.Id));

            _logger.LogInformation($"User {user.NickName} with id: {user.Guid} has been disconnected!");

            return Task.FromResult(s_empty);
        }
    }
}
