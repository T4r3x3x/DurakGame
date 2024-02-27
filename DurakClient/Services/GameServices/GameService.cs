using AutoMapper;

using Connections.Services;

using GameEngine.Entities.GameEntities;

using System;
using System.Threading.Tasks;

using static Connections.Services.GameService;

namespace DurakClient.Services.GameServices
{
    public class GameService : IGameService
    {
        private readonly GameServiceClient _gameService;
        private readonly IMapper _mapper;
        private readonly ActionRequest _actionRequest;

        public GameService(GameServiceClient gameService, IMapper mapper, Guid userId, Guid lobbyId)
        {
            _gameService = gameService;
            _mapper = mapper;

            _actionRequest = new()
            {
                LobbyId = lobbyId.ToString(),
                SenderId = userId.ToString(),
            };
        }

        public async Task EndTurn() => await _gameService.EndTurnAsync(_actionRequest);

        public async Task GiveUp() => await _gameService.GiveUpAsync(_actionRequest);

        public async Task ThrowDeffenceCard(Card card, int position)
        {
            var cardMessage = _mapper.Map<CardMessage>(card);
            var requset = new ThrowDefenceCardRequest()
            {
                ActionRequest = _actionRequest,
                Card = cardMessage,
                Position = position
            };
            await _gameService.ThrowDeffenceCardAsync(requset);
        }
        public async Task ThrowAttackCard(Card card)
        {
            var cardMessage = _mapper.Map<CardMessage>(card);
            var request = new ThrowAttackCardRequest()
            {
                ActionRequest = _actionRequest,
                Card = cardMessage
            };
            await _gameService.ThrowAttackCardAsync(request);
        }
    }
}