using Connections.Services;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

namespace Server.Services
{
    public class LobbyService : Connections.Services.LobbyService.LobbyServiceBase
    {
        public override Task CreateLobby(LobbyCreateRequest request, IServerStreamWriter<LobbyState> responseStream, ServerCallContext context)
        {
            return base.CreateLobby(request, responseStream, context);
        }

        public override Task<Empty> DeleteLobby(ActionRequest request, ServerCallContext context)
        {
            return base.DeleteLobby(request, context);
        }

        public override Task GetAllLobies(Empty request, IServerStreamWriter<LobbyList> responseStream, ServerCallContext context)
        {
            return base.GetAllLobies(request, responseStream, context);
        }

        public override Task JoinLobby(ActionRequest request, IServerStreamWriter<LobbyState> responseStream, ServerCallContext context)
        {
            return base.JoinLobby(request, responseStream, context);
        }

        public override Task<Empty> KickPlayer(KickPlayerRequest request, ServerCallContext context)
        {
            return base.KickPlayer(request, context);
        }

        public override Task<Empty> LeaveLobby(ActionRequest request, ServerCallContext context)
        {
            return base.LeaveLobby(request, context);
        }

        public override Task<Empty> PrepareToGame(ActionRequest request, ServerCallContext context)
        {
            return base.PrepareToGame(request, context);
        }

        public override Task<Empty> StartGame(ActionRequest request, ServerCallContext context)
        {
            return base.StartGame(request, context);
        }
    }
}
