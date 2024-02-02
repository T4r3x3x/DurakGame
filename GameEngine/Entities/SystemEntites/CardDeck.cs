using GameEngine.Entities.GameEntities;

namespace GameEngine.Entities.SystemEntites
{
    public class CardDeck
    {
        private Stack<Card> _cardsDeck;

        public CardDeck(Stack<Card> cards) => _cardsDeck = cards;

        public Card GetCard() => _cardsDeck.Pop();

        public List<Card> GetCards(int count)
        {
            if (count > _cardsDeck.Count)
                count = _cardsDeck.Count;

            var cards = new List<Card>();
            for (int i = 0; i < count; i++)
                cards.Add(_cardsDeck.Pop());
            return cards;
        }

        public bool HasAnyCard() => _cardsDeck.Any();
    }
}