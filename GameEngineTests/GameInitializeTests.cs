using GameEngine;

namespace GameEngineTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [TestCase(4)]
        [TestCase(1)]
        [TestCase(6)]
        [TestCase(3)]
        public void SHOULD_CREATE_GAME_ACCORDING_PLAYERS_COUNT(int playersCount)
        {
            GameSettings gameSettings = new GameSettings() { DeckType = GameEngine.Entities.DeckType.Common, PlayersCount = playersCount, PlayersStartCardsCount = 6 };
            GameManager game = new(gameSettings);

            game.StartGame();

            Assert.AreEqual(game.Players.Count, gameSettings.PlayersCount);
        }

        [TestCase(4)]
        [TestCase(1)]
        [TestCase(6)]
        [TestCase(3)]
        public void SHOULD_CREATE_GAME_ACCORDING_CARDS_COUNT(int cardsCount)
        {
            GameSettings gameSettings = new GameSettings() { DeckType = GameEngine.Entities.DeckType.Common, PlayersCount = 5, PlayersStartCardsCount = cardsCount };
            GameManager game = new(gameSettings);

            game.StartGame();

            Assert.AreEqual(game.Players[0].Cards.Count, gameSettings.PlayersStartCardsCount);
        }


        [TestCase(50, ExpectedResult = 2)]
        [TestCase(52, ExpectedResult = 0)]
        public int CREATE_GAME_ACCORDING_CARDS_COUNT_SHOULD_THROW_EXCEPTION(int cardsCount)//говно надо отдельно протестировать CardDeck
        {
            GameSettings gameSettings = new GameSettings() { DeckType = GameEngine.Entities.DeckType.Extended, PlayersCount = 5, PlayersStartCardsCount = cardsCount };
            GameManager game = new(gameSettings);

            game.StartGame();

            return game.Players[1].Cards.Count;
        }
    }
}