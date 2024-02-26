using ReactiveUI;

using System;

namespace DurakClient.MVVM.ViewModels
{
    public class LobbyViewModel : ViewModelBase, IRoutableViewModel
    {
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public IScreen HostScreen { get; }

        public LobbyViewModel(IScreen hostScreen)
        {
            HostScreen = hostScreen;
        }
    }
}
