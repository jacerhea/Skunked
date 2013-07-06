﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage.Rules;

namespace Cribbage.Player
{
    public class ConsolePlayer: PlayerBase, ICribPlayer
    {
        public ConsolePlayer(string name) : base(name)
        {
        }

        public List<Card> DealHand(IList<Card> hand)
        {
            var sortedHand = hand.OrderBy(c => c.Rank).ThenBy(c => c.Suit).ToList();

            Console.WriteLine("You have been dealt the following hand:");

            int indexDealt = 1;
            foreach (var card in sortedHand)
            {
                Console.WriteLine(string.Format("{0}: {1}", indexDealt, card));
                indexDealt++;
            }

            int firstCardIndex = -1;

            while (firstCardIndex < 1 || firstCardIndex > sortedHand.Count)
            {
                firstCardIndex = GetInt("Please choose the first valid card:");
            }

            int secondCardIndex = -1;

            while (secondCardIndex < 1 || secondCardIndex > sortedHand.Count)
            {
                secondCardIndex = GetInt("Please choose the second valid card:");
            }

            var returnCards = new List<Card>(2) { sortedHand[firstCardIndex - 1], sortedHand[secondCardIndex - 1] };

            return returnCards;
        }

        public Card PlayShow(CribGameRules gameRules, List<Card> pile, List<Card> handLeft)
        {
            Console.WriteLine("The following cards are in the pile:");

            int indexDealt = 1;
            foreach (var card in pile)
            {
                Console.WriteLine(string.Format("{0}: {1}", indexDealt, card));
                indexDealt++;
            }

            Console.WriteLine("The following cards are left to play:");

            int cardsleftToPlayIndex = 1;
            foreach (var card in pile)
            {
                Console.WriteLine(string.Format("{0}: {1}", cardsleftToPlayIndex, card));
                cardsleftToPlayIndex++;
            }

            int firstCardIndex = -1;

            while (firstCardIndex < 1 || firstCardIndex > handLeft.Count)
            {
                firstCardIndex = GetInt("Please choose the first valid card:");
            }

            return handLeft[firstCardIndex - 1];
        }

        public Card ChooseCard(List<Card> cardsToChoose)
        {
            if (cardsToChoose == null) throw new ArgumentNullException("deck");
            var randomGenerator = new Random();
            var randomIndex = randomGenerator.Next(cardsToChoose.Count);
            return cardsToChoose[randomIndex];            
        }

        public int CountHand(Card card, IEnumerable<Card> hand)
        {
            while (true)
            {
                Console.Write("Count your hands' score,");
                string enterText = Console.ReadLine();

                int enteredValue;
                if (int.TryParse(enterText, out enteredValue))
                {
                    return enteredValue;
                }
            }
        }

        public bool Equals(ICribPlayer other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public int GetInt(string questionToAsk)
        {
            while (true)
            {
                Console.Write(questionToAsk);
                string enterText = Console.ReadLine();

                int enteredValue;
                if (int.TryParse(enterText, out enteredValue))
                {
                    return enteredValue;
                }
            }
        }
    }
}