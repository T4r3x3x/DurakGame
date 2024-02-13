//using GameEngine.Entities.GameEntities;

//namespace GameEngine.Entities.SystemEntites
//{
//    public class PlayersCollection
//    {
//        private readonly List<Player> Players;
//        private int _mainAttackerIndex;

//        public event Action AllPlayersIsDone;

//        public Player this[int index]
//        {
//            get => Players[index];
//            set => Players[index] = value;
//        }

//        public PlayersCollection(List<Player> players)
//        {
//            Players = players;
//        }

//        public void Remove(Player player) => Players.Remove(player);

//        public int GetDefenderCardsCount() => Players[_mainAttackerIndex].Cards.Count();

//        public void NextTurn()
//        {
//        }

//        public bool IsAllPlayersDone()
//        {
//            foreach (var player in Players)
//                if (player.IsDone == false)
//                    return false;

//            return true;
//        }

//        public Player Next()
//        {

//        }
//        private void CheckRanOutCardsPlayers()
//        {
//            if (_deck.HasAnyCard())
//                return;

//            foreach (var player in PlayersCollection.Players)
//                if (player.Cards.Count() == 0)
//                    Players.Remove(player);
//            if (Players.Count < 2)
//                OnEndGame();
//        }

//        private void SetStartRoles()
//        {
//            foreach (var player in PlayersCollection.Players)
//                player.Role = Player.PlayerRole.SubAttacker;

//            var choosen = ChooseFirstAttacker();
//            choosen.Role = Player.PlayerRole.MainAttacker;
//        }

//        private int GetDeffencePlayerCardsCount()
//        {
//            return Players.Where(player => player.Role == Player.PlayerRole.Defender).First().Cards.Count();
//        }

//        private void SwitchRoles()
//        {
//            var previousMainAttacker = GetPreviousMainAttacker();
//            previousMainAttacker.Role = Player.PlayerRole.SubAttacker;

//            var previousMainAttackerIndex = Players.IndexOf(previousMainAttacker);

//            var newMainAttackerIndex = GetNextPlayerIndex(previousMainAttackerIndex);
//            Players[newMainAttackerIndex].Role = Player.PlayerRole.MainAttacker;

//            var newDeffenderIndex = GetNextPlayerIndex(newMainAttackerIndex);
//            Players[newDeffenderIndex].Role = Player.PlayerRole.Defender;
//        }

//        private Player GetPreviousMainAttacker()
//        {
//            return Players.Find(player => player.Role == Player.PlayerRole.MainAttacker)!;
//        }

//        private int GetNextPlayerIndex(int currentPlayerIndex)
//        {
//            var nextPlayerIndex = currentPlayerIndex++;
//            if (nextPlayerIndex > Players.Co)
//                nextPlayerIndex = 0;

//            return nextPlayerIndex;
//        }
//    }
//}
