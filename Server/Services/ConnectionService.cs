using Connections.Services;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Server.Entities;

namespace Server.Services
{
    public class ConnectionService : Connections.Services.ConnectionService.ConnectionServiceBase
    {
        private static readonly Empty s_empty = new Empty();
        private readonly ConnectionResources _resources;

        public ConnectionService(ConnectionResources resources)
        {
            _resources = resources;
        }

        public override Task<PlayerId> Connect(LoginRequest request, ServerCallContext context)
        {
            Guid playerId = Guid.NewGuid();
            User player = new User { Guid = playerId, NickName = request.NickName };
            _resources.Users.Add(playerId, player);

            return Task.FromResult(new PlayerId { Id = playerId.ToString() });
        }

        public override Task<Empty> Disconnect(PlayerId request, ServerCallContext context)
        {
            var user = _resources.GetUser(request.Id);
            _resources.Users.Remove(Guid.Parse(request.Id));
            return Task.FromResult(s_empty);
        }
    }
}
