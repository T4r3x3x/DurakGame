﻿using DurakClient.MVVM.Models;
using DurakClient.Services;
using DurakClient.Services.LobbyServices;
using DurakClient.Utilities;
using DurakClient.Utilities.Ranges;

using GameEngine.Entities.SystemEntites;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using System;
using System.Reactive;

namespace DurakClient.MVVM.ViewModels
{
    public class CreateLobbyViewModel : ViewModelBase
    {
        private readonly ILobbyService _lobbyService;
        private readonly Resources _resources;

        [Reactive] public string Name { get; set; } = DefaultValuesProvider.DefaultLobbyName;
        [Reactive] public string Password { get; set; } = DefaultValuesProvider.DefaultPassword;
        [Reactive] public int PlayersCount { get; set; } = DefaultValuesProvider.PlayersCountRange.Min;
        [Reactive] public int CardsStartCount { get; set; } = DefaultValuesProvider.CardsStartCountRange.Min;
        [Reactive] public DeckType DeckType { get; set; } = DefaultValuesProvider.DefaultDeckTypeValue;
        public IntRange PlayersCountRange { get; set; } = DefaultValuesProvider.PlayersCountRange;
        public IntRange CardsStartCountRange { get; set; } = DefaultValuesProvider.CardsStartCountRange;

        public ReactiveCommand<Unit, Unit> ResetCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CreateLobbyCommand { get; set; }

        public CreateLobbyViewModel(ILobbyService lobbyService, Resources resources)
        {
            _lobbyService = lobbyService;
            _resources = resources;

            ResetCommand = ReactiveCommand.Create(Reset);
            CreateLobbyCommand = ReactiveCommand.Create(CreateLobby, IsLobbyNameValid);
        }
        private IObservable<bool> IsLobbyNameValid => this.WhenAnyValue(x => x.Name,
                                                               x => !string.IsNullOrWhiteSpace(x) && x.Length > 5);
        private void Reset()
        {
            Name = DefaultValuesProvider.DefaultLobbyName;
            PlayersCount = DefaultValuesProvider.PlayersCountRange.Min;
            CardsStartCount = DefaultValuesProvider.CardsStartCountRange.Min;
            DeckType = DefaultValuesProvider.DefaultDeckTypeValue;
        }

        private void CreateLobby()
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
            _lobbyService.CreateLobby(lobbyCreateModel);
        }
    }
}