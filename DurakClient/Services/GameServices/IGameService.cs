using GameEngine.Entities.GameEntities;

using System.Threading.Tasks;

namespace DurakClient.Services.GameServices
{
    public interface IGameService
    {
        public Task EndTurn();
        public Task GiveUp();
        public Task ThrowDeffenceCard(Card card, int position);
        public Task ThrowAttackCard(Card card);
    }
}