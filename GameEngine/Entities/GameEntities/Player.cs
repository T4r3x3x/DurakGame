namespace GameEngine.Entities.GameEntities
{
    public partial class Player
    {
        private readonly int _startCardsCount;

        private readonly List<Card> _cards;

        public PlayerRole Role;
        public bool IsDone = false;

        public Player(int startCardsCount)
        {
            _startCardsCount = startCardsCount;
            _cards = new List<Card>(_startCardsCount);
        }

        public void AddCards(List<Card> cards)
        {
            _cards.AddRange(cards);
        }

        public void RemoveCard(Card card)
        {
            _cards.Remove(card);
        }

        public IEnumerable<Card> Cards => _cards;

        //public void SwitchRole()
        //{
        //    switch (Role)
        //    {
        //        case PlayerRole.Defender: Role = PlayerRole.Attacker; break;
        //        case PlayerRole.Attacker: Role = PlayerRole.Defender; break;
        //    }
        //}

        public int NeededCardsCount()
        {
            var neededCardsCount = _startCardsCount - _cards.Count;

            if (neededCardsCount < 0)
                return 0;

            return neededCardsCount;
        }

        public enum PlayerRole
        {
            Defender, SubAttacker, MainAttacker,
        }
    }
}