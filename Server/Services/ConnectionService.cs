using Connections.Services;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Server.Entities;

namespace Server.Services
{
    public class ConnectionService : Connections.Services.ConnectionService.ConnectionServiceBase
    {
        public override Task<PlayerId> Connect(LoginRequest request, ServerCallContext context)
        {
            Guid playerId = Guid.NewGuid();
            User player = new User { Guid = playerId, NickName = request.NickName };
            Resources.Users.Add(playerId, player);

            return Task.FromResult(new PlayerId { Id = playerId.ToString() });
        }

        public override Task<Empty> Disconnect(PlayerId request, ServerCallContext context)
        {
            Resources.Users.Remove(Guid.Parse(request.Id));
            return base.Disconnect(request, context);
        }
    }
}
