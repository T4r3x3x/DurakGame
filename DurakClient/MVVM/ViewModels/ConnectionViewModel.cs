using DurakClient.Services.ConnectionServices;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using System;
using System.Reactive;

namespace DurakClient.MVVM.ViewModels;

public class ConnectionViewModel : ViewModelBase, IScreen
{
    [Reactive] public string Nickname { get; set; }
    public RoutingState Router { get; } = new RoutingState();

    private readonly IConnectionService _connectionService;

    public ConnectionViewModel(IConnectionService connectionService)
    {
        _connectionService = connectionService;

        ConnectCommand = ReactiveCommand.Create(() =>
        {
            _connectionService.Connect(Nickname);
            Router.Navigate.Execute(new LobbyViewModel(this));
        },
        isNicknameValid);
    }

    public ReactiveCommand<Unit, Unit> ConnectCommand { get; set; }

    private IObservable<bool> isNicknameValid => this.WhenAnyValue(
                x => x.Nickname,
                x => !string.IsNullOrWhiteSpace(x) && x.Length > 5
                );
}