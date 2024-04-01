using DurakClient.Factories.ViewModelFactories;
using DurakClient.MVVM.Models;
using DurakClient.Services;
using DurakClient.Services.LobbyServices;
using DurakClient.Utilities;
using DurakClient.Utilities.Ranges;

using GameEngine.Entities.SystemEntites;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using Splat;

using System;
using System.Reactive;

namespace DurakClient.MVVM.ViewModels
{
    public class CreateLobbyViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly ILobbyService _lobbyService;
        private readonly Resources _resources;
        private readonly IViewModelFactory<LobbyViewModel> _lobbyViewModelFactory;

        [Reactive] public string Name { get; set; } = DefaultValuesProvider.DefaultLobbyName;
        [Reactive] public string Password { get; set; } = DefaultValuesProvider.DefaultPassword;
        [Reactive] public int PlayersCount { get; set; } = DefaultValuesProvider.PlayersCountRange.Min;
        [Reactive] public int CardsStartCount { get; set; } = DefaultValuesProvider.CardsStartCountRange.Min;
        [Reactive] public DeckType DeckType { get; set; } = DefaultValuesProvider.DefaultDeckTypeValue;
        public IntRange PlayersCountRange { get; set; } = DefaultValuesProvider.PlayersCountRange;
        public IntRange CardsStartCountRange { get; set; } = DefaultValuesProvider.CardsStartCountRange;
        public string? UrlPathSegment => "Create lobby";
        public IScreen HostScreen { get; }

        public ReactiveCommand<Unit, Unit> ResetCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CreateLobbyCommand { get; set; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; set; }

        public CreateLobbyViewModel(ILobbyService lobbyService, Resources resources, IScreen hostScreen, IViewModelFactory<LobbyViewModel> lobbyViewModelFactory = null!)
        {
            HostScreen = hostScreen;
            _lobbyService = lobbyService;
            _resources = resources;
            HostScreen = hostScreen;
            _lobbyViewModelFactory = lobbyViewModelFactory ?? Locator.Current.GetService<IViewModelFactory<LobbyViewModel>>()!;

            ResetCommand = ReactiveCommand.Create(Reset);
            CreateLobbyCommand = ReactiveCommand.Create(CreateLobby, IsLobbyNameValid);
            GoBackCommand = ReactiveCommand.Create(GoBack);
        }
        private IObservable<bool> IsLobbyNameValid => this.WhenAnyValue(x => x.Name,
                                                               x => !string.IsNullOrWhiteSpace(x) && x.Length > 5);

        private void GoBack()
        {
            HostScreen.Router.NavigateBack.Execute();
        }

        private void Reset()
        {
            Name = DefaultValuesProvider.DefaultLobbyName;
            PlayersCount = DefaultValuesProvider.PlayersCountRange.Min;
            CardsStartCount = DefaultValuesProvider.CardsStartCountRange.Min;
            DeckType = DefaultValuesProvider.DefaultDeckTypeValue;
        }

        private async void CreateLobby()
        {
            CreateLobbyModel lobbyCreateModel = new CreateLobbyModel()
            {
                CreatorId = _resources.PlayerId,
                GameSettings = new GameSettings()
                {
                    DeckType = DeckType,
                    PlayersCount = PlayersCount,
                    PlayersStartCardsCount = CardsStartCount,
                },
                Name = Name,
                Password = Password,
            };
            await _lobbyService.CreateLobby(lobbyCreateModel);

            HostScreen.Router.Navigate.Execute(_lobbyViewModelFactory.GetViewModel(HostScreen));
        }
    }
}