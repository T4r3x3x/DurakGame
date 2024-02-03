using GameEngine.Entities.GameEntities;

namespace GameEngine.FirstPlayerChoosers
{
    public class BegginerChooser : IBegginerChooser
    {
        public Player ChooseFirstAttacker(List<Player> players, Card.Suit trumpSuit)
        {
            var playersAndTrumps = GetPlayerAndMinTrump(players, trumpSuit);
            var result = playersAndTrumps.OrderBy(x => x.minTrumpCard?.RankValue).FirstOrDefault();

            if (result.trumpCardOnwer == null)
                return players[0];

            return result.trumpCardOnwer;
        }

        private List<(Player trumpCardOnwer, Card? minTrumpCard)> GetPlayerAndMinTrump(List<Player> players, Card.Suit trumpSuit)
        {
            List<(Player trumpCardOnwer, Card? minTrumpCard)> result = new();

            foreach (var player in players)
                result.Add((player, GetMinTrumpCard(player, trumpSuit)));

            return result;
        }

        private Card? GetMinTrumpCard(Player player, Card.Suit trumpSuit)
        {
            return player.Cards.Where(card => card.SuitValue == trumpSuit).OrderBy(card => card.RankValue).FirstOrDefault();
        }
    }
}
