using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using System;
using System.Reactive;

namespace DurakClient.MVVM.ViewModels;

public class MainViewModel : ViewModelBase, IScreen
{
    [Reactive] public string Nickname { get; set; }

    public ReactiveCommand<Unit, Unit> ConnectCommand { get; set; }

    private IObservable<bool> isNicknameValid => this.WhenAnyValue(
                x => x.Nickname,
                x => !string.IsNullOrWhiteSpace(x) && x.Length > 5
                );

    public RoutingState Router { get; } = new RoutingState();

    public MainViewModel()
    {

        ConnectCommand = ReactiveCommand.Create(() =>
        {
            // var connect = new ConnectionModel() { Nickname = Nickname };
            // connect.Connect();
            Router.Navigate.Execute(new LobbyViewModel(this));
        }, isNicknameValid);
    }
}
