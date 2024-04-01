using Server.Entities;
using Server.Services;

using System.Collections.Concurrent;

namespace Server.CustomCollections
{
    public class LobbyDictionary : NotifyingConcurrentDictionary<Guid, Lobby>
    {
        public LobbyDictionary() { }
        public LobbyDictionary(ConcurrentDictionary<Guid, Lobby> dictionary) : base(dictionary) { }

        public override bool TryAdd(Guid key, Lobby value)
        {
            var result = base.TryAdd(key, value);
            if (result)
                value.Players.OnDataChanged += Update;
            return result;
        }

        public override bool Remove(Guid key, out Lobby value)
        {
            var result = base.Remove(key, out value);
            if (result)
                value.Players.OnDataChanged -= Update;
            return result;
        }

        private void Update(IEnumerable<User> users) => OnDataChanged();
    }
}