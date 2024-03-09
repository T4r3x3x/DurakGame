using DurakClient.MVVM.Models;
using DurakClient.Services.LobbyServices;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using Splat;

using System.Collections.Generic;
using System.Reactive;

namespace DurakClient.MVVM.ViewModels
{
    public class LobbiesViewModel : ViewModelBase, IRoutableViewModel
    {
        [Reactive]
        public List<Lobby> Lobbies { get; private set; } = [new Lobby()
        {
            Name = "First",
            HasPassword = false,
            JoinedPlayersCount = 0,
            Settings = new()
            {
                DeckType = GameEngine.Entities.SystemEntites.DeckType.Common,
                PlayersCount = 2,
                PlayersStartCardsCount = 3
            }
        },
            new Lobby()
            {
                Name = "Second",
                HasPassword = true,
                JoinedPlayersCount = 3,
                Settings = new()
                {
                    DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                    PlayersCount = 4,
                    PlayersStartCardsCount = 5
                }
            },
            new Lobby()
            {
                Name = "Second",
                HasPassword = true,
                JoinedPlayersCount = 3,
                Settings = new()
                {
                    DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                    PlayersCount = 4,
                    PlayersStartCardsCount = 5
                }
            },
            new Lobby()
            {
                Name = "Second",
                HasPassword = true,
                JoinedPlayersCount = 3,
                Settings = new()
                {
                    DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                    PlayersCount = 4,
                    PlayersStartCardsCount = 5
                }
            },
            new Lobby()
            {
                Name = "Second",
                HasPassword = true,
                JoinedPlayersCount = 3,
                Settings = new()
                {
                    DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                    PlayersCount = 4,
                    PlayersStartCardsCount = 5
                }
            },
            new Lobby()
            {
                Name = "Second",
                HasPassword = true,
                JoinedPlayersCount = 3,
                Settings = new()
                {
                    DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                    PlayersCount = 4,
                    PlayersStartCardsCount = 5
                }
            },
            new Lobby()
            {
                Name = "Second",
                HasPassword = true,
                JoinedPlayersCount = 3,
                Settings = new()
                {
                    DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                    PlayersCount = 4,
                    PlayersStartCardsCount = 5
                }
            },
            new Lobby()
            {
                Name = "Second",
                HasPassword = true,
                JoinedPlayersCount = 3,
                Settings = new()
                {
                    DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                    PlayersCount = 4,
                    PlayersStartCardsCount = 5
                }
            },
            new Lobby()
            {
                Name = "Second",
                HasPassword = true,
                JoinedPlayersCount = 3,
                Settings = new()
                {
                    DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                    PlayersCount = 4,
                    PlayersStartCardsCount = 5
                }
            },
            new Lobby()
            {
                Name = "Second",
                HasPassword = true,
                JoinedPlayersCount = 3,
                Settings = new()
                {
                    DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                    PlayersCount = 4,
                    PlayersStartCardsCount = 5
                }
            },
            new Lobby()
            {
                Name = "Second",
                HasPassword = true,
                JoinedPlayersCount = 3,
                Settings = new()
                {
                    DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                    PlayersCount = 4,
                    PlayersStartCardsCount = 5
                }
            },
            new Lobby()
            {
                Name = "Second",
                HasPassword = true,
                JoinedPlayersCount = 3,
                Settings = new()
                {
                    DeckType = GameEngine.Entities.SystemEntites.DeckType.Extended,
                    PlayersCount = 4,
                    PlayersStartCardsCount = 5
                }
            },
        ];
        public string UrlPathSegment { get; } = "Lobbies list";
        public IScreen HostScreen { get; }

        private readonly ILobbyService _lobbyService;

        public LobbiesViewModel(IScreen hostScreen, ILobbyService lobbyService = null)
        {
            HostScreen = hostScreen;
            _lobbyService = lobbyService ?? Locator.Current.GetService<ILobbyService>();
            GetAllLobbies = ReactiveCommand.Create(GetAllLobies);
        }

        public ReactiveCommand<Unit, Unit> GetAllLobbies { get; }

        private async void GetAllLobies() => Lobbies = await _lobbyService.GetAllLobby();
    }
}