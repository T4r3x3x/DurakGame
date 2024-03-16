using DurakClient.Services.LobbyServices;
using DurakClient.Utilities;

using GameEngine.Entities.SystemEntites;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using System.Reactive;

namespace DurakClient.MVVM.ViewModels
{
    public class CreateLobbyViewModel : ViewModelBase
    {
        private readonly ILobbyService _lobbyService;

        [Reactive] public string Name { get; set; } = DefaultValues.DefaultName;
        [Reactive] public int MinPlayersCount { get; set; } = DefaultValues.DefaultMinPlayersCount;
        [Reactive] public int MaxPlayersCount { get; set; } = DefaultValues.DefaultMaxPlayersCount;
        [Reactive] public int MinStartCardsCount { get; set; } = DefaultValues.DefaultMinStartCardsCount;
        [Reactive] public int MaxStartCardsCount { get; set; } = DefaultValues.DefaultMaxStartCardsCount;
        [Reactive] public DeckType DeckType { get; set; } = DefaultValues.DefaultDeckTypeValue;
        public ReactiveCommand<Unit, Unit> ResetCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CreateLobbyCommand { get; set; }

        public CreateLobbyViewModel()
        {
        }

        private void Reset()
        {

        }

        private void CreateLobby()
        {

        }
    }
}