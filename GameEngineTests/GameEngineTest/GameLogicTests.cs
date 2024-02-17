using GameEngine.Entities.GameEntities;
using GameEngine.Entities.SystemEntites;
using GameEngine.FirstPlayerChoosers;

namespace GameEngineTests.GameEngineTest
{
    internal class GameLogicTests
    {

        #region Change roles
        #endregion

        #region Translate
        [Test]
        public void DEFENDER_TRUYING_TO_TRANSLATE_SHOULD_TRANSLATE()
        {
            #region Arrange
            Card diamondsTwo = new Card(Card.Suit.Diamonds, Card.Rank.Two);
            Card clubsTwo = new Card(Card.Suit.Clubs, Card.Rank.Two);

            var istance = InstanceGameWithEmptyCardDeck(2);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var players = istance.players;
            players[0].Role = Player.PlayerRole.MainAttacker;
            players[1].Role = Player.PlayerRole.Defender;

            turnCards.AddAttackCard(diamondsTwo);
            #endregion

            #region Act
            gameManager.ThrowAttackCard(players[1], clubsTwo);
            #endregion

            #region Assert       
            int currentAttackCardsCount = turnCards.AttackCardsCount;
            Assert.IsTrue(currentAttackCardsCount == 2);

            //todo сделать отдельный тест на смену ролей? 
            Assert.IsTrue(players[0].Role == Player.PlayerRole.Defender);
            Assert.IsTrue(players[1].Role == Player.PlayerRole.MainAttacker);
            #endregion
        }

        [Test]
        public void DEFENDER_TRYING_TO_TRANSLATE_SHOULD_NOT_TRANSLATE()
        {
            #region Arrange
            Card diamondsTwo = new Card(Card.Suit.Diamonds, Card.Rank.Two);
            Card clubsTwo = new Card(Card.Suit.Clubs, Card.Rank.Three);

            var istance = InstanceGameWithEmptyCardDeck(2);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var players = istance.players;
            players[0].Role = Player.PlayerRole.MainAttacker;
            players[1].Role = Player.PlayerRole.Defender;

            turnCards.AddAttackCard(diamondsTwo);
            #endregion

            #region Act
            gameManager.ThrowAttackCard(players[1], clubsTwo);
            #endregion

            #region Assert       
            int currentAttackCardsCount = turnCards.AttackCardsCount;
            Assert.IsFalse(currentAttackCardsCount == 2);

            //todo сделать отдельный тест на смену ролей? 
            Assert.IsFalse(players[0].Role == Player.PlayerRole.Defender);
            Assert.IsFalse(players[1].Role == Player.PlayerRole.MainAttacker);
            #endregion
        }
        #endregion

        #region Turn ending
        [Test]
        public void ALL_ATTACKERS_IS_DONE()
        {
            #region Arrange
            Card diamondsTwo = new Card(Card.Suit.Diamonds, Card.Rank.Two);
            Card clubsTwo = new Card(Card.Suit.Clubs, Card.Rank.Two);

            (Game gameManager, TurnCards turnCards, List<Player> players) istance = InstanceGameWithEmptyCardDeck(2);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var players = istance.players;
            players[0].Role = Player.PlayerRole.MainAttacker;
            players[1].Role = Player.PlayerRole.Defender;

            players[0].AddCards([diamondsTwo]);
            players[1].AddCards([diamondsTwo]);
            turnCards.AddAttackCard(diamondsTwo);
            turnCards.AddDeffenceCard(diamondsTwo, 0);
            #endregion

            #region Act
            gameManager.EndTurn(players[0]);
            #endregion

            #region Assert       
            int currentAttackCardsCount = turnCards.AttackCardsCount;
            Assert.IsTrue(players[0].Cards.First() == diamondsTwo);

            //todo сделать отдельный тест на смену ролей? 
            Assert.IsTrue(players[0].Role == Player.PlayerRole.Defender);
            Assert.IsTrue(players[1].Role == Player.PlayerRole.MainAttacker);
            #endregion
        }

        [Test]
        public void DEFENDER_GIVE_UP()
        {
            #region Arrange
            Card diamondsTwo = new Card(Card.Suit.Diamonds, Card.Rank.Two);
            Card clubsTwo = new Card(Card.Suit.Clubs, Card.Rank.Two);

            (Game gameManager, TurnCards turnCards, List<Player> players) istance = InstanceGameWithEmptyCardDeck(2);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var players = istance.players;
            players[0].Role = Player.PlayerRole.MainAttacker;
            players[1].Role = Player.PlayerRole.Defender;

            players[0].AddCards([diamondsTwo]);

            turnCards.AddAttackCard(diamondsTwo);
            #endregion

            #region Act
            gameManager.GiveUp(players[1]);
            #endregion

            #region Assert       
            int currentAttackCardsCount = turnCards.AttackCardsCount;
            Assert.IsTrue(players[0].Cards.First() == diamondsTwo);

            //todo сделать отдельный тест на смену ролей? 
            Assert.IsTrue(players[0].Role == Player.PlayerRole.Defender);
            Assert.IsTrue(players[1].Role == Player.PlayerRole.MainAttacker);
            #endregion
        }

        [Test]
        public void TWO_PLAYERS_NEXT_TURN_CHANGE_ROLES()
        {
            #region Arrange
            Card diamondsTwo = new Card(Card.Suit.Diamonds, Card.Rank.Two);
            Card clubsTwo = new Card(Card.Suit.Clubs, Card.Rank.Two);

            var istance = InstanceGameWithEmptyCardDeck(2);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var players = istance.players;
            players[0].Role = Player.PlayerRole.MainAttacker;
            players[1].Role = Player.PlayerRole.Defender;

            turnCards.AddAttackCard(diamondsTwo);
            #endregion

            #region Act
            gameManager.ThrowAttackCard(players[1], clubsTwo);
            #endregion

            #region Assert       
            Assert.IsTrue(players[0].Role == Player.PlayerRole.Defender);
            Assert.IsTrue(players[1].Role == Player.PlayerRole.MainAttacker);
            #endregion
        }

        [Test]
        public void THREE_PLAYERS_NEXT_TURN_CHANGE_ROLES()
        {
            #region Arrange
            Card diamondsTwo = new Card(Card.Suit.Diamonds, Card.Rank.Two);
            Card clubsTwo = new Card(Card.Suit.Clubs, Card.Rank.Two);

            var istance = InstanceGameWithEmptyCardDeck(3);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var players = istance.players;
            players[0].Role = Player.PlayerRole.MainAttacker;
            players[1].Role = Player.PlayerRole.Defender;
            players[2].Role = Player.PlayerRole.SubAttacker;

            turnCards.AddAttackCard(diamondsTwo);
            #endregion

            #region Act
            gameManager.ThrowAttackCard(players[1], clubsTwo);
            #endregion

            #region Assert       
            int currentAttackCardsCount = turnCards.AttackCardsCount;
            Assert.IsTrue(currentAttackCardsCount == 2);

            //todo сделать отдельный тест на смену ролей? 
            Assert.IsTrue(players[0].Role == Player.PlayerRole.SubAttacker);
            Assert.IsTrue(players[1].Role == Player.PlayerRole.MainAttacker);
            Assert.IsTrue(players[2].Role == Player.PlayerRole.Defender);
            #endregion
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

            var istance = InstanceGameWithEmptyCardDeck(1);

            var turnCards = istance.turnCards;

            var gameManager = istance.gameManager;
            gameManager.GetType().GetProperty("TrumpCard")!.SetValue(gameManager, diamondsTwo);

            var player = istance.players[0];
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

            var istance = InstanceGameWithEmptyCardDeck(1);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var player = istance.players[0];
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

            var istance = InstanceGameWithEmptyCardDeck(1);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var player = istance.players[0];
            player.Role = Player.PlayerRole.MainAttacker;

            turnCards.AddAttackCard(diamondsTwo);

            int startAttackCardsCount = turnCards.AttackCardsCount;
            #endregion

            #region Act
            gameManager.ThrowAttackCard(player, diamondsThree);
            #endregion

            #region Assert       
            int currentAttackCardsCount = turnCards.AttackCardsCount;
            Assert.IsFalse(startAttackCardsCount == currentAttackCardsCount);
            #endregion
        }

        [Test]
        public void ATTACKER_TRYING_THROW_ADDITIONAL_CARD_SHOULD_ADD()
        {
            #region Arrange
            Card diamondsTwo = new Card(Card.Suit.Diamonds, Card.Rank.Two);
            Card diamondsThree = new Card(Card.Suit.Spades, Card.Rank.Two);

            var istance = InstanceGameWithEmptyCardDeck(1);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var player = istance.players[0];
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
            #region Arrange
            Card diamondsTwo = new Card(Card.Suit.Diamonds, Card.Rank.Two);
            Card clubsTwo = new Card(Card.Suit.Clubs, Card.Rank.Two);

            (Game gameManager, TurnCards turnCards, List<Player> players) istance = InstanceGameWithEmptyCardDeck(3);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var players = istance.players;
            players[0].Role = Player.PlayerRole.MainAttacker;
            players[1].Role = Player.PlayerRole.Defender;
            players[2].Role = Player.PlayerRole.SubAttacker;

            players[1].AddCards([diamondsTwo]);

            turnCards.AddAttackCard(diamondsTwo);
            #endregion

            #region Act
            gameManager.GiveUp(players[1]);
            #endregion

            #region Assert                

            Assert.IsTrue(players.Count() == 1);

            #endregion
        }

        [Test]
        public void ONE_OF_THREE_PLAYER_ENDED_GAME()
        {
            #region Arrange
            Card diamondsTwo = new Card(Card.Suit.Diamonds, Card.Rank.Two);
            Card clubsTwo = new Card(Card.Suit.Clubs, Card.Rank.Two);

            (Game gameManager, TurnCards turnCards, List<Player> players) istance = InstanceGameWithEmptyCardDeck(3);

            var turnCards = istance.turnCards;
            var gameManager = istance.gameManager;
            var players = istance.players;
            players[0].Role = Player.PlayerRole.MainAttacker;
            players[1].Role = Player.PlayerRole.Defender;
            players[2].Role = Player.PlayerRole.SubAttacker;

            players[0].AddCards([diamondsTwo]);
            players[1].AddCards([diamondsTwo]);

            turnCards.AddAttackCard(diamondsTwo);
            turnCards.AddDeffenceCard(diamondsTwo, 0);
            #endregion

            #region Act
            gameManager.GiveUp(players[1]);
            #endregion

            #region Assert                

            Assert.IsTrue(players.Count() == 2);

            #endregion
        }
        #endregion


        #region Tools

        private (Game gameManager, TurnCards turnCards, List<Player> players) InstanceGameWithEmptyCardDeck(int playersCount)
        {
            const int startCardsCount = 6;

            List<Player> players = new List<Player>();
            for (int i = 0; i < playersCount; i++)
            {
                Player player = new Player(startCardsCount);
                player.Role = Player.PlayerRole.SubAttacker;
                players.Add(player);
            }

            TurnCards turnCards = new TurnCards();
            CardDeck cardDeck = GetEmptyCardDeck();
            BegginerChooser firstPlayerChooser = new();

            Game gameManager = new Game(players, cardDeck, turnCards, firstPlayerChooser);

            return (gameManager, turnCards, players);
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
