using GameEngine.Entities.GameEntities;
using GameEngine.Entities.SystemEntites;
using GameEngine.Shufflers;

namespace GameEngine.Factories
{
    public class GameManagerFactory : IGameManagerFactory
    {
        public Game GetGameManager(GameSettings gameSettings, IShuffler<Card> shuffler)
        {
            CardDeckFactory cardDeckFactory = new(shuffler);
            CardDeck cardDeck = cardDeckFactory.GetCardDeck(gameSettings.DeckType);
            TurnCards turnCards = new TurnCards();
            List<Player> players = new List<Player>();
            for (int i = 0; i < gameSettings.PlayersCount; i++)
                players.Add(new(gameSettings.PlayersStartCardsCount));

            Game gameManager = new GameManager(players, cardDeck, turnCards);

            return gameManager;
        }
    }
}