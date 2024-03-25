using ReactiveUI;

using System.Reactive;

namespace DurakClient.MVVM.ViewModels
{
    internal class LobbyViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> KickPlayerCommand { get; }
        public ReactiveCommand<Unit, Unit> GetReadyCommand { get; }
        public ReactiveCommand<Unit, Unit> LeaveCommand { get; }
        public ReactiveCommand<Unit, Unit> StartGameCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteLobbyCommand { get; }
    }
}