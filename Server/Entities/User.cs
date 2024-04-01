namespace Server.Services
{
    public class User
    {
        private bool _areReady = false;
        public required Guid Guid { get; set; }
        public required string NickName { get; set; }
        public bool AreReady
        {
            get => _areReady;
            set
            {
                if (_areReady != value)
                {
                    _areReady = value;
                    OnDataChanged?.Invoke();
                }
            }
        }

        public event Action OnDataChanged;
    }
}