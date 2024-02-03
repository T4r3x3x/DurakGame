using GameEngine.Entities.GameEntities;
using GameEngine.Entities.SystemEntites;
using GameEngine.FirstPlayerChoosers;
using GameEngine.Shufflers;

namespace GameEngine.Factories
{
    public class GameFactory : IGameManagerFactory
    {
        public Game GetGameManager(GameSettings gameSettings, IShuffler<Card> shuffler)
        {
            CardDeckFactory cardDeckFactory = new(shuffler);
            CardDeck cardDeck = cardDeckFactory.GetCardDeck(gameSettings.DeckType);
            TurnCards turnCards = new TurnCards();
            IBegginerChooser firstPlayerChooser = new BegginerChooser();

            List<Player> players = GetPlayers(gameSettings);

            Game gameManager = new Game(players, cardDeck, turnCards, firstPlayerChooser);

            return gameManager;
        }

        private List<Player> GetPlayers(GameSettings gameSettings)
        {
            List<Player> players = new List<Player>();
            for (int i = 0; i < gameSettings.PlayersCount; i++)
            {
                var player = new Player(gameSettings.PlayersStartCardsCount);
                player.Role = Player.PlayerRole.SubAttacker;
                players.Add(player);
            }
            return players;
        }
    }
}