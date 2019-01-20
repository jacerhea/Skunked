﻿using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.Players
{
    public class TestPlayer : IEquatable<TestPlayer>, IGameRunnerPlayer
    {
        private readonly ScoreCalculator _calculator = new ScoreCalculator();

        public TestPlayer(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; private set; }
        public int Id { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }



        /// <summary>
        /// Deal Hand and return cards that will go back in crib
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>Set of Cards to throw in crib.</returns>
        public List<Card> DealHand(IList<Card> hand)
        {
            var handCopy = hand.ToList();
            handCopy.Shuffle();
            return handCopy.Take(2).ToList();
        }

        public Card PlayShow(GameRules gameRules, List<Card> pile, List<Card> handLeft)
        {
            if (gameRules == null) throw new ArgumentNullException(nameof(gameRules));
            if (pile == null) throw new ArgumentNullException(nameof(pile));
            if (handLeft == null) throw new ArgumentNullException(nameof(handLeft));
            if (handLeft.Count == 0) throw new ArgumentException("handLeft");

            return handLeft.Single();
        }

        public Card ChooseCard(List<Card> cardsToChoose)
        {
            if (cardsToChoose == null) throw new ArgumentNullException(nameof(cardsToChoose));
            var randomIndex = RandomProvider.GetThreadRandom().Next(0, cardsToChoose.Count - 1);
            return cardsToChoose[randomIndex];
        }

        public int CountHand(Card card, IEnumerable<Card> hand)
        {
            return _calculator.CountShowScore(card, hand).Score;
        }

        public bool Equals(TestPlayer other)
        {
            return other?.Id == Id;
        }
    }
}