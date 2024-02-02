namespace GameEngine.Entities.GameEntities
{
    public class Card
    {
        public readonly Suit SuitValue;
        public readonly Rank RankValue;

        public Card(Suit suit, Rank rank)
        {
            SuitValue = suit;
            RankValue = rank;
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