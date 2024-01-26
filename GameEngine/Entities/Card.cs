namespace GameEngine.Entities
{
    public class Card
    {
        public readonly Suit suit;
        public readonly Rank rank;

        public Card(Suit suit, Rank rank)
        {
            this.suit = suit;
            this.rank = rank;
        }

        public enum Suit
        {
            Diamonds,
            Hearts,
            Clubs,
            Spades,
        }

        public enum Rank
        {
            Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace,
        }
    }
}