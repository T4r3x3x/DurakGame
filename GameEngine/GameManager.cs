using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace DurakForms
{
    static internal class GameManager //всё что связано непросредственно с самой игрой (козыри, чей ход и т.д.)
    {

        static internal Card.Suit trump;
        static internal Stack<Card> DeckOfCards = new Stack<Card>();

        static Random rand = new Random();

        static internal List<List<Card>> turnCards = new List<List<Card>>(2);//1 - attacker's cards, 0 - defender's cards

        static internal List<Player> players = new List<Player>(2);

        static int cards_max_count_at_current_turn = 6;

        static internal bool isPlaying = false;
        static internal Player winner;
        static internal Card trump_card;

        static public void StartGame()
        {
            isPlaying = true;
            turnCards.Add(new List<Card>());
            turnCards.Add(new List<Card>());
            int i = 0;
            foreach (var suit in Enum.GetValues(typeof(Card.Suit)))
            {
                foreach (var rank in Enum.GetValues(typeof(Card.Rank)))
                {
                    DeckOfCards.Push(new Card((Card.Suit)suit, (Card.Rank)rank));
                    i++;
                }
            }
            Reshuffle();
            players[1].role = Player.Role.Attacker;
            players[0].role = Player.Role.Defender;
            //DeckOfCards.Clear();
        }

        static void Reshuffle() //Fisher–Yates shuffle
        {
            List<Card> tempList = DeckOfCards.ToList();
            Card temp;
            int j;
            for (int i = 35; i > 0; i--)
            {
                j = rand.Next(i + 1);
                temp = tempList[i];
                tempList[i] = tempList[j];
                tempList[j] = temp;
            }
            players.Add(new Player(tempList.GetRange(tempList.Count() - 6, 6)));
            tempList.RemoveRange(tempList.Count() - 6, 6);
            players.Add(new Player(tempList.GetRange(tempList.Count() - 6, 6)));
            tempList.RemoveRange(tempList.Count() - 6, 6);

            trump_card = tempList.Last();
            
            Card temp_card = tempList.First();
            tempList[0] = tempList.Last();
            tempList[tempList.Count() - 1] = temp_card;
            trump = temp_card.suit;

            Interface.SendStartData(((int)trump_card.suit).ToString() + ((int)trump_card.rank).ToString());
            foreach (var item in tempList)
                DeckOfCards.Push(item);
        }

        public static void EndTurn(Player player)
        {
            if (player.role == Player.Role.Attacker)
            {
                if(turnCards[(int)Player.Role.Attacker].Count() != 0)
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


        static void GiveUp(Player player)
        {
            player.AddCards(turnCards[(int)Player.Role.Defender]);
            foreach (var card in turnCards[(int)Player.Role.Defender])
            {
                if (card == null)
                    player.cards.Remove(card);
            }
            player.AddCards(turnCards[(int)Player.Role.Attacker]);
        }

        static void SwitchRoles()
        {
            foreach (var player in players)
            {
                player.SwitchRole();
            }
        }

        static void NextTurn()
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

        static void GiveCards()
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


        static void EndGame(Player player)
        {
            winner = player;
            isPlaying = false;
        }

        static public Card GetCard()
        {
            return DeckOfCards.Pop();
        }

        static public void ThrowCard(Card card, Player player, int position)//Для защищающегося 
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
        static public void ThrowCard(Card card, Player player)//Для нападающего
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

        static bool CanTranslate(Card card)
        {
            foreach (var _card in turnCards[(int)Player.Role.Attacker])
                if (card.rank != _card.rank)
                    return false;
            return true;
        }

        static void Translate(Card card)
        {
            SwitchRoles();
            turnCards[(int)Player.Role.Attacker].Add(card);
            turnCards[(int)Player.Role.Defender].Add(null);
        }

        static bool CanThrow(Card card)
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

        static bool ComparingCards(Card attacker, Card defender)
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
