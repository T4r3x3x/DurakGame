using GameEngine.Entities.GameEntities;

namespace GameEngine.FirstPlayerChoosers
{
    public interface IBegginerChooser
    {
        Player ChooseFirstAttacker(List<Player> players, Card.Suit trumpSuit);
    }
}
