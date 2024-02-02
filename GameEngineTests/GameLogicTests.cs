using GameEngine.Entities.GameEntities;
using GameEngine.Entities.SystemEntites;

namespace GameEngineTests
{
    internal class GameLogicTests
    {

        #region Translate
        [Test]
        public void DEFENDER_TRUYING_TO_TRANSLATE_SHOULD_NOT_TRANSLATE()//todo проверить что сменились роли, и что карты переместилсь адекватно
        {
            #region Arrange
            Card diamondsTwo = new Card(Card.Suit.Diamonds, Card.Rank.Two);
            Card clubsTwo = new Card(Card.Suit.Clubs, Card.Rank.Two);

            var istance = InstanceGameWithOneAttackCard(diamondsTwo);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var player = istance.player;
            player.Role = Player.PlayerRole.Defender;

            turnCards.AddAttackCard(diamondsTwo);

            int startAttackCardsCount = turnCards.AttackCardsCount;
            #endregion

            #region Act
            gameManager.ThrowAttackCard(player, clubsTwo);
            #endregion

            #region Assert       
            int currentAttackCardsCount = turnCards.AttackCardsCount;
            Assert.IsTrue(currentAttackCardsCount == 2);
            #endregion
        }

        [Test]
        public void DEFENDER_TRYING_TO_TRANSLATE_SHOULD_TRANSLATE()
        {

        }
        #endregion

        #region Turn ending
        [Test]
        public void ALL_ATTACKERS_IS_DONE()
        {

        }

        [Test]
        public void DEFENDER_GIVE_UP()
        {

        }

        [Test]
        public void TWO_PLAYERS_NEXT_TURN_CHANGE_ROLES()
        {

        }

        [Test]
        public void THREE_PLAYERS_NEXT_TURN_CHANGE_ROLES()
        {

        }

        #endregion

        #region Defense
        [Test]
        public void DEFENDER_TRYING_FILLED_CARD_SHOULD_NOT_FILL()
        {
            #region Arrange
            const int cardPosition = 0;

            Card diamondsTwo = new Card(Card.Suit.Diamonds, Card.Rank.Two);
            Card diamondsThree = new Card(Card.Suit.Spades, Card.Rank.Three);

            var istance = InstanceGameWithOneAttackCard(diamondsTwo);

            var turnCards = istance.turnCards;

            var gameManager = istance.gameManager;
            gameManager.GetType().GetProperty("TrumpCard").SetValue(gameManager, diamondsTwo);

            var player = istance.player;
            player.Role = Player.PlayerRole.Defender;

            turnCards.AddAttackCard(diamondsTwo);
            #endregion

            #region Act
            gameManager.ThrowDeffenceCard(player, diamondsThree, cardPosition);
            #endregion

            #region Assert                 
            Assert.IsFalse(turnCards.IsCardFilled(cardPosition));
            #endregion
        }

        [Test]
        public void DEFENDER_TRYING_FILLED_CARD_SHOULD_FILL()
        {
            #region Arrange
            const int cardPosition = 0;

            Card diamondsTwo = new Card(Card.Suit.Diamonds, Card.Rank.Two);
            Card diamondsThree = new Card(Card.Suit.Diamonds, Card.Rank.Three);

            var istance = InstanceGameWithOneAttackCard(diamondsTwo);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var player = istance.player;
            player.Role = Player.PlayerRole.Defender;

            turnCards.AddAttackCard(diamondsTwo);
            #endregion

            #region Act
            gameManager.ThrowDeffenceCard(player, diamondsThree, cardPosition);
            #endregion

            #region Assert       
            Assert.IsTrue(turnCards.IsCardFilled(cardPosition));
            #endregion

        }
        #endregion

        #region Attack
        [Test]
        public void TRYING_THROW_ADDITIONAL_CARD_SHOULD_NOT_ADD()
        {
            #region Arrange
            Card diamondsTwo = new Card(Card.Suit.Diamonds, Card.Rank.Two);
            Card diamondsThree = new Card(Card.Suit.Diamonds, Card.Rank.Three);

            var istance = InstanceGameWithOneAttackCard(diamondsTwo);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var player = istance.player;
            player.Role = Player.PlayerRole.MainAttacker;

            turnCards.AddAttackCard(diamondsTwo);

            int startAttackCardsCount = turnCards.AttackCardsCount;
            #endregion

            #region Act
            gameManager.ThrowAttackCard(player, diamondsThree);
            #endregion

            #region Assert       
            int currentAttackCardsCount = turnCards.AttackCardsCount;
            Assert.IsTrue(startAttackCardsCount == currentAttackCardsCount);
            #endregion
        }

        [Test]
        public void ATTACKER_TRYING_THROW_ADDITIONAL_CARD_SHOULD_ADD()
        {
            #region Arrange
            Card diamondsTwo = new Card(Card.Suit.Diamonds, Card.Rank.Two);
            Card diamondsThree = new Card(Card.Suit.Spades, Card.Rank.Two);

            var istance = InstanceGameWithOneAttackCard(diamondsTwo);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var player = istance.player;
            player.Role = Player.PlayerRole.MainAttacker;

            turnCards.AddAttackCard(diamondsTwo);

            int startAttackCardsCount = turnCards.AttackCardsCount;
            #endregion

            #region Act
            gameManager.ThrowAttackCard(player, diamondsThree);
            #endregion

            #region Assert       
            int currentAttackCardsCount = turnCards.AttackCardsCount;
            Assert.IsTrue(startAttackCardsCount != currentAttackCardsCount);
            #endregion
        }
        #endregion

        #region Game ending
        [Test]
        public void ALL_PLAYERS_EXCEPT_ONE_ENDED_GAME()
        {

        }
        [Test]
        public void ONE_OF_THREE_PLAYER_ENDED_GAME()
        {

        }
        #endregion


        #region Tools

        private (Game gameManager, TurnCards turnCards, Player player) InstanceGameWithOneAttackCard(Card card)
        {
            const int startCardsCount = 6;

            Player player = new Player(startCardsCount);

            TurnCards turnCards = new TurnCards();
            CardDeck cardDeck = GetEmptyCardDeck();

            Game gameManager = new GameManager([player], cardDeck, turnCards);

            return (gameManager, turnCards, player);
        }
        private List<Player> GetPlayers(int playersCount, int startCardsCount)
        {
            List<Player> players = new(playersCount);
            for (int i = 0; i < playersCount; i++)
                players.Add(new(startCardsCount));

            return players;
        }
        private CardDeck GetEmptyCardDeck() => new(new Stack<Card>([]));
        #endregion
    }
}
