using GameEngine.Entities.GameEntities;

namespace GameEngine.Entities.SystemEntites
{
    public class TurnCards
    {
        private List<Card> _attackCards = new List<Card>();
        private List<Card> _deffenceCards = new List<Card>();

        public IEnumerable<Card> AttackCards => _attackCards;
        public IEnumerable<Card> DeffenceCards => _deffenceCards;
        //todo внедрить сюда отслеживание сколько возможно кинуть защищающемуся карт? 

        public void AddAttackCard(Card card)
        {
            _attackCards.Add(card);
            _deffenceCards.Add(null);
        }

        public void AddDeffenceCard(Card card, int position)
        {
            _deffenceCards[position] = card;
        }

        public int AttackCardsCount => _attackCards.Count;

        public Card GetAttackCard(int position) => _attackCards[position];

        public bool HasAnyCard => _attackCards.Any();

        public bool AllCardsFilled => _attackCards.Count == _deffenceCards.Count;

        public bool IsDeffenceStarted => _deffenceCards.Where(card => card is not null).Count() > 0;

        public bool HasCardWithRank(Card.Rank rank) => _attackCards.Concat(_deffenceCards).Where(card => card?.RankValue == rank).Count() > 0;

        public bool IsCardFilled(int position) => _deffenceCards[position] is not null;

        public List<Card> GetAll() => _attackCards.Concat(_deffenceCards).ToList();

        public void Clear()
        {
            _attackCards.Clear();
            _deffenceCards.Clear();
        }

        public void Translate()
        {
            _attackCards.AddRange(_deffenceCards);
            _deffenceCards.Clear();
            _deffenceCards.AddRange(Enumerable.Repeat<Card>(null, _attackCards.Count));
        }
    }
}
