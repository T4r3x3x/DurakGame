using GameEngine.Entities.GameEntities;
using GameEngine.Entities.SystemEntites;
using GameEngine.Shufflers;

namespace GameEngine.Factories
{
    internal class CardDeckFactory(IShuffler<Card> _shuffler)
    {
        private const int ExtendedRanskCount = 4;

        public CardDeck GetCardDeck(DeckType deckType)
        {
            var cardsList = GetCardColletion(deckType);
            _shuffler.Shuffle(cardsList);
            var cardsStack = new Stack<Card>(cardsList);

            return new(cardsStack);
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
    }
}