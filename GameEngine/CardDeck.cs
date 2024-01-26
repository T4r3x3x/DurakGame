using GameEngine.Entities;

namespace GameEngine
{
    public class CardDeck
    {
        private const int ExtendedRanskCount = 4;
        private Stack<Card> _cardsDeck = new Stack<Card>();

        public CardDeck(DeckType deckType) => CreateDeck(deckType);

        public Card GetCard() => _cardsDeck.Peek();

        private void CreateDeck(DeckType deckType)
        {
            var cards = GetCardColletion(deckType);
            Shuffle(cards);
            _cardsDeck = new Stack<Card>(cards);
        }

        private List<Card> GetCardColletion(DeckType deckType)
        {
            List<Card> cards = new List<Card>();

            foreach (Card.Suit suit in Enum.GetValues(typeof(Card.Suit)))
                foreach (Card.Rank rank in Enum.GetValues(typeof(Card.Rank)))
                    cards.Add(new Card(suit, rank));

            if (deckType == DeckType.Common)
                DeleteExtendedCards(cards);

            return cards;
        }

        private void DeleteExtendedCards(List<Card> cards)
        {
            foreach (var suit in Enum.GetValues(typeof(Card.Suit)))
                for (int i = 0; i < ExtendedRanskCount; i++)
                    cards.Remove(new Card((Card.Suit)suit, (Card.Rank)i));
        }

        /// <summary>
        /// Fisher–Yates algorithm
        /// </summary>
        /// <param name="cards"></param>
        private void Shuffle(List<Card> cards)
        {
            Random random = new Random();

            for (int i = cards.Count; i > 0; i--)
            {
                var j = random.Next(i + 1);
                (cards[i], cards[j]) = (cards[j], cards[i]);
            }
        }
    }
}