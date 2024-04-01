using Server.Services;

using System.Collections;

namespace Server.CustomCollections
{
    public class PlayersList : IEnumerable<User>
    {
        private List<User> _users;

        public int Count => _users.Count;
        public Action<IEnumerable<User>> OnDataChanged;

        public PlayersList(int capacity)
        {
            _users = new List<User>(capacity);
        }

        public bool TryAdd(User user)
        {
            if (_users.Count == _users.Capacity)
                return false;

            _users.Add(user);
            user.OnDataChanged += OnUserChanged;

            OnDataChanged?.Invoke(_users);
            return true;
        }

        public bool TryRemove(User user)
        {
            var removeResult = _users.Remove(user);
            if (removeResult)
            {
                OnDataChanged?.Invoke(_users);
                user.OnDataChanged -= OnUserChanged;
            }
            return removeResult;
        }

        public int IndexOf(User user) => _users.IndexOf(user);

        public IEnumerator<User> GetEnumerator()
        {
            return ((IEnumerable<User>)_users).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _users.GetEnumerator();
        }
        private void OnUserChanged()
        {
            OnDataChanged?.Invoke(this);
        }
    }
}