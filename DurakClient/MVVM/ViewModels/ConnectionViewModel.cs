﻿using DurakClient.Services;
using DurakClient.Services.ConnectionServices;

using ReactiveUI;

using System;
using System.Reactive;
using System.Text.RegularExpressions;

namespace DurakClient.MVVM.ViewModels;

public class ConnectionViewModel : ViewModelBase, IScreen
{
    private string _nickname = string.Empty;
    public string Nickname
    {
        get => _nickname;
        set
        {
            //так как AsciiOnly не работает, удаляем ручками всё, что не латинские буквы.
            var regex = new Regex(@"[^a-zA-Z]");
            value = regex.Replace(value, string.Empty);
            this.RaiseAndSetIfChanged(ref _nickname, value);
        }
    }

    public RoutingState Router { get; } = new RoutingState();

    private readonly IConnectionService _connectionService;
    private readonly Resources _resources;

    public ConnectionViewModel(IConnectionService connectionService, Resources resources)
    {
        _connectionService = connectionService;
        _resources = resources;
        ConnectCommand = ReactiveCommand.Create(Connect, isNicknameValid);
    }

    public ReactiveCommand<Unit, Unit> ConnectCommand { get; set; }

    private async void Connect()
    {
        var guid = await _connectionService.Connect(Nickname!);
        _resources.Guid = guid;
        Router.Navigate.Execute(new LobbiesViewModel(this)); //можно просто внедрять viewModel на которую будем переходить
    }

    private IObservable<bool> isNicknameValid => this.WhenAnyValue(
                x => x.Nickname,
                x => !string.IsNullOrWhiteSpace(x) && x.Length > 5
                );
}