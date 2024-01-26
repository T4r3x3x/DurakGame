namespace GameEngine.Entities
{
    public partial class Player
    {
        private const int StartCardsCount = 6;

        public List<Card> cards = new List<Card>(StartCardsCount);
        public Role role;

        public Player(List<Card> cards)
        {
            this.cards = cards;
        }

        public void SwitchRole()
        {
            switch (role)
            {
                case Role.Defender: role = Role.Attacker; break;
                case Role.Attacker: role = Role.Defender; break;
            }
        }
        public enum Role
        {
            Defender, Attacker,
        }
    }
}