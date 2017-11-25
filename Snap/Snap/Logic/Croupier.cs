using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSSocketClient
{
    public class Croupier
    {
        public List<Card> GenerateDeck()
        {
            List<Card> cardsToReturn = new List<Card>();
            foreach (Colors color in Enum.GetValues(typeof(Colors)))
            {
                foreach (Figures figure in Enum.GetValues(typeof(Figures)))
                {
                    Card cartToAdd = new Card(figure, color);
                    cardsToReturn.Add(cartToAdd);
                }
            }
            var rnd = new Random();
            var shuffledCards = cardsToReturn.OrderBy(card => rnd.Next()).ToList();
            return shuffledCards;
        }

        public void ShowCards(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                Console.WriteLine(card.color + "  " + card.figure);
            }
        }

        public List<Card> DealHand(List<Card> cards)
        {
            List<Card> handToGive = new List<Card>();

            Card first = GetOneCardFromDeck(cards);
            Card second = GetOneCardFromDeck(cards);

            handToGive.Add(first);
            handToGive.Add(second);
            return handToGive;
        }

        public Card GetOneCardFromDeck(List<Card> cards)
        {
            Card card = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            return card;
        }

        public List<Card> DealFlop(List<Card> cards)
        {
            List<Card> flop = new List<Card>();
            //without burned one
            Card first = GetOneCardFromDeck(cards);
            Card second = GetOneCardFromDeck(cards);
            Card third = GetOneCardFromDeck(cards);

            flop.Add(first);
            flop.Add(second);
            flop.Add(third);
            return flop;
        }

        public List<Card> DealTurn(List<Card> cards)
        {
            List<Card> turn = new List<Card>();

            Card first = GetOneCardFromDeck(cards);
            turn.Add(first);
            return turn;
        }

        public List<Card> DealRiver(List<Card> cards)
        {
            List<Card> river = new List<Card>();

            Card first = GetOneCardFromDeck(cards);
            river.Add(first);
            return river;
        }
    }
}
