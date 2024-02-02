using GameEngine.Entities.GameEntities;

namespace GameEngine.Entities.SystemEntites
{
    public class GameManager
    {
        private int _maxAttackCards = 6;

        private CardDeck _deck;
        private TurnCards _turnCards;

        public readonly List<Player> Players;

        public virtual Card TrumpCard { get; private set; }
        public virtual Card.Suit TrumpSuit { get => TrumpCard.SuitValue; }

        public event Action OnEndGame;
        public event Func<Player> OnPlayerEndedGame;

        public GameManager(List<Player> players, CardDeck deck, TurnCards turnCards)
        {
            _deck = deck;
            _turnCards = turnCards;
            Players = players;
        }

        #region API
        public void StartGame()
        {
            SetTrump();
            GiveCards();
            SetStartRoles();
        }

        private void SetTrump() => TrumpCard = _deck.GetCard();

        public void EndTurn(Player player)
        {
            player.IsDone = true;

            if (player.Role != Player.PlayerRole.Defender)
                if (_turnCards.HasAnyCard)
                    if (_turnCards.AllCardsFilled)
                        if (IsAllPlayersDone())
                            NextTurn();
        }

        private bool IsAllPlayersDone()
        {
            foreach (var player in Players)
                if (player.IsDone == false)
                    return false;

            return true;
        }

        public void GiveUp(Player player)
        {
            if (player.Role != Player.PlayerRole.Defender)
                return;

            var cards = _turnCards.GetAll();
            player.AddCards(cards);

            NextTurn();
        }

        public virtual void ThrowDeffenceCard(Player cardOnwer, Card card, int position)
        {
            if (cardOnwer.Role == Player.PlayerRole.Defender)
                if (position < _turnCards.AttackCardsCount)
                    if (!_turnCards.IsCardFilled(position))
                        if (CanFill(_turnCards.GetAttackCard(position), card))
                            PlaceDeffenceCard(cardOnwer, card, position);
        }


        public void ThrowAttackCard(Player cardOwner, Card card)
        {
            if (cardOwner.Role != Player.PlayerRole.Defender)
            {
                if (CanThrow(cardOwner, card))
                    PlaceAttackCard(cardOwner, card);
            }
            else
            {
                if (CanTranslate(card))
                    Translate(cardOwner, card);
            }
        }
        #endregion

        private void PlaceDeffenceCard(Player cardOwner, Card card, int position)
        {
            _turnCards.AddDeffenceCard(card, position);
            cardOwner.RemoveCard(card);
        }

        private void PlaceAttackCard(Player cardOwner, Card card)
        {
            _turnCards.AddAttackCard(card);
            cardOwner.RemoveCard(card);
        }

        private void SetStartRoles()
        {
            foreach (var player in Players)
                player.Role = Player.PlayerRole.SubAttacker;

            var choosen = ChooseFirstAttacker();
            choosen.Role = Player.PlayerRole.MainAttacker;
        }

        private Player ChooseFirstAttacker()
        {
            var playersAndTrumps = GetPlayerAndMinTrump();
            var result = playersAndTrumps.OrderBy(x => x.minTrumpCard?.RankValue).FirstOrDefault();

            if (result.trumpCardOnwer == null)
                return Players[0];

            return result.trumpCardOnwer;
        }

        private List<(Player trumpCardOnwer, Card? minTrumpCard)> GetPlayerAndMinTrump()
        {
            List<(Player trumpCardOnwer, Card? minTrumpCard)> result = new();

            foreach (var player in Players)
                result.Add((player, GetMinTrumpCard(player)));

            return result;
        }

        private Card? GetMinTrumpCard(Player player)
        {
            return player.Cards.Where(card => card.SuitValue == TrumpSuit).OrderBy(card => card.RankValue).FirstOrDefault();
        }

        private void SwitchRoles()
        {
            var previousMainAttacker = GetPreviousMainAttacker();
            previousMainAttacker.Role = Player.PlayerRole.SubAttacker;

            var previousMainAttackerIndex = Players.IndexOf(previousMainAttacker);

            var newMainAttackerIndex = GetNextPlayerIndex(previousMainAttackerIndex);
            Players[newMainAttackerIndex].Role = Player.PlayerRole.MainAttacker;

            var newDeffenderIndex = GetNextPlayerIndex(newMainAttackerIndex);
            Players[newDeffenderIndex].Role = Player.PlayerRole.Defender;
        }

        private Player GetPreviousMainAttacker()
        {
            return Players.Find(player => player.Role == Player.PlayerRole.MainAttacker)!;
        }

        private int GetNextPlayerIndex(int currentPlayerIndex)
        {
            var nextPlayerIndex = currentPlayerIndex++;
            if (nextPlayerIndex > Players.Count)
                nextPlayerIndex = 0;

            return nextPlayerIndex;
        }

        private void NextTurn()
        {
            _turnCards.Clear();
            GiveCards();
            CheckRanOutCardsPlayers();
            SwitchRoles();
            _maxAttackCards = GetDeffencePlayerCardsCount();
        }

        private void CheckRanOutCardsPlayers()
        {
            if (_deck.HasAnyCard())
                return;

            foreach (var player in Players)
                if (player.Cards.Count() == 0)
                    Players.Remove(player);
            if (Players.Count < 2)
                OnEndGame();
        }

        private int GetDeffencePlayerCardsCount()
        {
            return Players.Where(player => player.Role == Player.PlayerRole.Defender).First().Cards.Count();
        }

        private void GiveCards()
        {
            if (!_deck.HasAnyCard())
                return;

            foreach (var player in Players)
            {
                var neededCardsCount = player.NeededCardsCount();
                var cards = _deck.GetCards(neededCardsCount);
                player.AddCards(cards);
            }
        }

        private bool CanTranslate(Card card)
        {
            if (_turnCards.IsDeffenceStarted)
                return false;

            return _turnCards.HasCardWithRank(card.RankValue);
        }

        private void Translate(Player player, Card card)
        {
            player.RemoveCard(card);
            _turnCards.AddAttackCard(card);
            SwitchRoles();
            //_turnCards.Translate();
        }

        private bool CanThrow(Player cardOwner, Card card)
        {
            if (!_turnCards.IsDeffenceStarted)
                return cardOwner.Role == Player.PlayerRole.MainAttacker;

            if (_turnCards.HasCardWithRank(card.RankValue))
                if (_turnCards.AttackCardsCount < _maxAttackCards)
                    return true;

            return false;
        }

        private bool CanFill(Card fillingCard, Card candidate)
        {
            if (candidate.SuitValue == fillingCard.SuitValue)
            {
                if (candidate.RankValue > fillingCard.RankValue)
                    return true;
            }
            else if (IsTrump(candidate))
                return true;

            return false;
        }

        private bool IsTrump(Card candidate)
        {
            return candidate.SuitValue == TrumpSuit;
        }
    }
}