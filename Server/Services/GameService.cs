﻿using AutoMapper;

using Connections.Services;

using GameEngine.Entities.GameEntities;
using GameEngine.Entities.SystemEntites;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Server.Entities;

using GameEntities = GameEngine.Entities.GameEntities;

namespace Server.Services
{
    public class GameService : Connections.Services.GameService.GameServiceBase
    {
        private readonly IMapper _mapper;

        public GameService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task StartGame(Game game) => game.StartGame();

        public override Task<Empty> EndTurn(ActionRequest request, ServerCallContext context)
        {
            (var game, var player) = GetGameEntities(request.LobbyId, request.LobbyId);
            game!.EndTurn(player);
            return base.EndTurn(request, context);
        }

        public override Task<Empty> GiveUp(ActionRequest request, ServerCallContext context)
        {
            (var game, var player) = GetGameEntities(request.LobbyId, request.LobbyId);
            game!.GiveUp(player);
            return base.GiveUp(request, context);
        }

        public override Task<Empty> StartGame(GameId request, ServerCallContext context)
        {
            return base.StartGame(request, context);
        }

        public override Task<Empty> ThrowAttackCard(ThrowAttackCardRequest request, ServerCallContext context)
        {
            (var game, var player) = GetGameEntities(request.ActionRequest.LobbyId, request.ActionRequest.LobbyId);
            GameEntities.Card card = FindCard(request.Card, player);
            game!.ThrowAttackCard(player, card);
            return base.ThrowAttackCard(request, context);
        }

        private GameEntities.Card FindCard(Connections.Services.Card messageCard, Player player)
        {
            var playingCard = _mapper.Map<GameEntities.Card>(messageCard);
            var card = player.Cards.Where(x => x == playingCard).SingleOrDefault();
            if (card == null)
                new RpcException(new Status(StatusCode.NotFound, $"Can't find a card [{messageCard.Rank},{messageCard.Suit}]"));

            return card!;
        }

        public override Task<Empty> ThrowDeffenceCard(ThrowDefenceCardRequest request, ServerCallContext context)
        {
            (var game, var player) = GetGameEntities(request.ActionRequest.LobbyId, request.ActionRequest.LobbyId);
            GameEntities.Card card = FindCard(request.Card, player);
            game.ThrowDeffenceCard(player, card, request.Position);
            return base.ThrowDeffenceCard(request, context);
        }

        private (Game, Player) GetGameEntities(string lobbyId, string userId)
        {
            var lobby = Resources.GetLobby(lobbyId);
            var user = Resources.GetUser(userId);
            var player = GetGameSidePlayer(lobby, user);

            if (lobby.Game == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Can't find a game in this lobby: {lobbyId}"));

            return (lobby.Game, player);
        }


        private GameEntities.Player GetGameSidePlayer(Lobby lobby, User user)
        {
            var index = lobby.Players.IndexOf(user);
            var gamePlayer = lobby.Game!.Players[index];

            return gamePlayer;
        }
    }
}
