using AutoMapper;

using Connections.Services;

using GameEngine.Entities.SystemEntites;
using GameEngine.Factories;
using GameEngine.Shufflers;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Server.Entities;

using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Server.Services
{
    public class LobbyService : Connections.Services.LobbyService.LobbyServiceBase
    {
        private readonly IMapper _mapper;
        private readonly GameService _gameService;

        public LobbyService(IMapper mapper, GameService gameService)
        {
            _mapper = mapper;
            _gameService = gameService;
        }

        public override Task CreateLobby(LobbyCreateRequest request, IServerStreamWriter<LobbyState> responseStream, ServerCallContext context)
        {
            GameSettings settings = _mapper.Map<GameSettings>(request.Settings);

            var creator = Resources.GetUser(request.CreatorId);

            Lobby lobby = new Lobby()
            {
                Guid = new Guid(),
                Owner = creator,
                Name = request.Name,
                Password = request.Password,
                Players = new ObservableCollection<User>(),
                Settings = settings
            };
            lobby.Players.Add(creator);
            Resources.Lobbies.Add(lobby.Guid, lobby);

            lobby.Players.CollectionChanged += OnLobbyStateChangedBehavior;

            //write
            //сделать отдельный сервис который стримит дату без контекста. Заебашить try catch{} если пользователь отключился просто дисконектим его 




            return null;
        }

        public void OnLobbyStateChangedBehavior(object? sender, NotifyCollectionChangedEventArgs e)
        {

        }

        public void Write(IServerStreamWriter<LobbyState> responseStream)
        {

        }

        public override Task<Empty> DeleteLobby(ActionRequest request, ServerCallContext context)
        {
            var guid = Resources.ParseGuid(request.LobbyId);
            Resources.Lobbies.Remove(guid);
            return base.DeleteLobby(request, context);
        }

        public override Task<LobbyList> GetAllLobies(Empty request, ServerCallContext context)
        {
            LobbyList lobbyList = new LobbyList();
            lobbyList.LobbyList_.AddRange(Resources.Lobbies.Select(x => _mapper.Map<LobbyModel>(x.Value)));
            return base.GetAllLobies(request, context);
        }

        public override Task JoinLobby(JoinRequest request, IServerStreamWriter<LobbyState> responseStream, ServerCallContext context)
        {
            var lobbyId = Resources.ParseGuid(request.ActionRequest.LobbyId);
            var playerId = Resources.ParseGuid(request.ActionRequest.SenderId);

            var playerSearchResult = Resources.Users.TryGetValue(playerId, out var player);
            if (!playerSearchResult)
                throw new RpcException(new Status(StatusCode.NotFound, $"Can't find a player with id: {playerId}"));

            var lobbySearchResult = Resources.Lobbies.TryGetValue(lobbyId, out var lobby);
            if (!lobbySearchResult)
                throw new RpcException(new Status(StatusCode.NotFound, $"Can't find a lobby with id: {lobbyId}"));

            if (request.Password != lobby!.Password)
                throw new RpcException(new Status(StatusCode.PermissionDenied, $"Wrong password!"));

            lobby!.Players.Add(player!);

            //write

            return null;
        }

        public override Task<Empty> KickPlayer(KickPlayerRequest request, ServerCallContext context)
        {
            var lobby = Resources.GetLobby(request.ActionRequest.LobbyId);
            var kickingPlayer = Resources.GetUser(request.KickingPlayerId);
            var kicker = Resources.GetUser(request.ActionRequest.SenderId);

            if (kicker != lobby!.Owner)
                throw new RpcException(new Status(StatusCode.PermissionDenied, "You are not onwer of the lobby!"));

            lobby!.Players.Remove(kickingPlayer!);

            return base.KickPlayer(request, context);
        }

        public override Task<Empty> LeaveLobby(ActionRequest request, ServerCallContext context)
        {
            var lobby = Resources.GetLobby(request.LobbyId);
            var player = Resources.GetUser(request.SenderId);

            lobby!.Players.Remove(player!);
            return base.LeaveLobby(request, context);
        }

        public override Task<Empty> PrepareToGame(ActionRequest request, ServerCallContext context)
        {
            var lobby = Resources.GetLobby(request.LobbyId);
            var player = Resources.GetUser(request.SenderId);
            lobby.Players.Where(x => x == player).First().AreReady = true;

            return base.PrepareToGame(request, context);
        }

        public override Task<Empty> StartGame(ActionRequest request, ServerCallContext context)
        {
            var lobby = Resources.GetLobby(request.LobbyId);
            var AreEverybodyIsReady = lobby.Players.Where(x => x.AreReady).Count() == lobby.Players.Count();
            if (!AreEverybodyIsReady)
                return Task.FromResult(new Empty());

            var gameFactory = new GameFactory();
            lobby.Game = gameFactory.GetGameManager(lobby.Settings, new FisherYatesShuffler<GameEngine.Entities.GameEntities.Card>());

            _gameService.StartGame(lobby.Game);
            return base.StartGame(request, context);
        }
    }
}