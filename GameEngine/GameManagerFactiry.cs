using GameEngine.Entities;

namespace GameEngine
{
    public class GameManagerFactiry : IGameManagerFactory
    {
        public GameManager GetGameManager(GameSettings gameSettings)
        {
            CardDeck cardDeck = new CardDeck(gameSettings.DeckType);
            TurnCards turnCards = new TurnCards();
            List<Player> players = new List<Player>();
            for (int i = 0; i < gameSettings.PlayersCount; i++)
                players.Add(new(gameSettings.PlayersStartCardsCount));

            GameManager gameManager = new GameManager(players, cardDeck, turnCards);

            return gameManager;
        }
    }
}