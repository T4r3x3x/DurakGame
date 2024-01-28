namespace GameEngine.Entities
{
    public partial class Player
    {
        private readonly int _startCardsCount;

        public readonly List<Card> Cards;
        public PlayerRole Role;
        public bool IsDone = false;

        public Player(int startCardsCount)
        {
            _startCardsCount = startCardsCount;
            Cards = new List<Card>(_startCardsCount);
        }

        public void SwitchRole()
        {
            switch (Role)
            {
                case PlayerRole.Defender: Role = PlayerRole.Attacker; break;
                case PlayerRole.Attacker: Role = PlayerRole.Defender; break;
            }
        }

        public int NeededCardsCount()
        {
            var neededCardsCount = _startCardsCount - Cards.Count;

            if (neededCardsCount < 0)
                return 0;

            return neededCardsCount;
        }

        public enum PlayerRole
        {
            Defender, Attacker,
        }
    }
}