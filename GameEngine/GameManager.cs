﻿using GameEngine.Entities;

namespace GameEngine
{
    public class GameManager
    {

        private Random _random = new Random();
        private int _maxAttackCards = 6;//todo не меняется со временем 

        public Card.Suit Trump;

        //todo вынести в отдельный класс?
        public List<List<Card>> TurnCards = new List<List<Card>>(2);//1 - attacker's cards, 0 - defender's cards

        //todo связать игровую сущность игрока с серверной (мб словарь?)
        public List<Player> Players;


        public bool IsPlaying = false;
        public Player Winner;
        public Card TrumpCard;

        public GameManager(GameSettings gameSettings)
        {

        }


        public void StartGame()
        {
            isPlaying = true;

            players[1].role = Player.Role.Attacker;
            players[0].role = Player.Role.Defender;
            //DeckOfCards.Clear();
        }



        public void EndTurn(Player player)
        {
            if (player.role == Player.Role.Attacker)
            {
                if (turnCards[(int)Player.Role.Attacker].Count() != 0)
                    if (turnCards[(int)Player.Role.Defender].Count(card => card == null) == 0) //если защищающийся покрыл все карты
                    {
                        NextTurn();
                        SwitchRoles();
                    }
            }
            else if (player.role == Player.Role.Defender)
            {
                GiveUp(player);
                NextTurn();
            }
        }


        void GiveUp(Player player)
        {
            player.AddCards(turnCards[(int)Player.Role.Defender]);
            foreach (var card in turnCards[(int)Player.Role.Defender])
            {
                if (card == null)
                    player.cards.Remove(card);
            }
            player.AddCards(turnCards[(int)Player.Role.Attacker]);
        }

        void SwitchRoles()
        {
            foreach (var player in players)
            {
                player.SwitchRole();
            }
        }

        void NextTurn()
        {
            turnCards[0].Clear();
            turnCards[1].Clear();

            //выдаём карты
            if (DeckOfCards.Count() != 0)//проверяем отсались ли карты  
                GiveCards();
            else
                foreach (var player in players)
                    if (player.cards_count == 0)
                    {
                        EndGame(player);
                        break;
                    }
        }

        void GiveCards()
        {
            int count_of_needed_cards = 0;

            foreach (var player in players)
                count_of_needed_cards += player.cards_count;

            foreach (var player in players)
                while (player.cards_count < 6)
                {
                    if (DeckOfCards.Count() == 0)
                        break;
                    player.AddCards(DeckOfCards.Pop());
                }
        }


        void EndGame(Player player)
        {
            winner = player;
            isPlaying = false;
        }

        public Card GetCard()
        {
            return DeckOfCards.Pop();
        }

        public void ThrowCard(Card card, Player player, int position)//Для защищающегося 
        {
            if (turnCards[(int)player.role].Count() > position)
            {
                if (turnCards[(int)player.role][position] == null)
                {
                    if (ComparingCards(turnCards[(int)Player.Role.Attacker][position], card))
                    {
                        turnCards[(int)player.role][position] = card;
                        player.cards.Remove(card);
                    }
                }
            }
        }
        public void ThrowCard(Card card, Player player)//Для нападающего
        {
            if (player.role == Player.Role.Attacker)
            {
                if (turnCards[(int)player.role].Count() < cards_max_count_at_current_turn)
                    if (CanThrow(card) || turnCards[(int)player.role].Count() == 0)
                    {
                        turnCards[(int)player.role].Add(card);
                        turnCards[0].Add(null);
                        player.cards.Remove(card);
                    }
            }
            else if (turnCards[(int)Player.Role.Defender].Count(card => card == null) == turnCards[(int)Player.Role.Defender].Count())//если в защиту не была кинута ни одна карта 
            {
                if (CanTranslate(card))
                {
                    player.cards.Remove(card);
                    Translate(card);
                }
            }
        }

        bool CanTranslate(Card card)
        {
            foreach (var _card in turnCards[(int)Player.Role.Attacker])
                if (card.rank != _card.rank)
                    return false;
            return true;
        }

        void Translate(Card card)
        {
            SwitchRoles();
            turnCards[(int)Player.Role.Attacker].Add(card);
            turnCards[(int)Player.Role.Defender].Add(null);
        }

        bool CanThrow(Card card)
        {
            foreach (var _card in turnCards[0])
            {
                if (_card != null)
                    if (_card.rank == card.rank)
                        return true;
            }
            foreach (var _card in turnCards[1])
            {
                if (_card.rank == card.rank)
                    return true;
            }
            return false;
        }

        bool ComparingCards(Card attacker, Card defender)
        {
            if (attacker.suit == defender.suit)
            {
                if (defender.rank > attacker.rank)
                    return true;
            }
            else if (defender.suit == trump)
                return true;
            return false;
        }

    }
}