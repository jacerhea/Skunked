﻿using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Cards;
using Skunked.Cards.Order;
using Skunked.Players;
using Skunked.Rules;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.Test.System
{
    public class TestPlayer : IEquatable<TestPlayer>, IGameRunnerPlayer
    {
        private readonly ScoreCalculator _calculator = new();

        public TestPlayer(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; }
        public int Id { get; }

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
        public List<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
        {
            var cardCountToThrow = hand.Count() - 4;
            var handCopy = hand.ToList();
            handCopy.Shuffle();
            return handCopy.Take(cardCountToThrow).ToList();
        }

        public Card DetermineCardsToPlay(GameRules gameRules, List<Card> pile, List<Card> handLeft)
        {
            if (gameRules == null) throw new ArgumentNullException(nameof(gameRules));
            if (pile == null) throw new ArgumentNullException(nameof(pile));
            if (handLeft == null) throw new ArgumentNullException(nameof(handLeft));
            if (handLeft.Count == 0) throw new ArgumentException(nameof(handLeft));

            return handLeft.OrderBy(card => card, RankComparer.Instance).First();
        }

        public Card CutCards(List<Card> cardsToChoose)
        {
            if (cardsToChoose == null) throw new ArgumentNullException(nameof(cardsToChoose));
            var randomIndex = RandomProvider.GetThreadRandom().Next(0, cardsToChoose.Count - 1);
            return cardsToChoose[randomIndex];
        }

        public int CountHand(Card card, IEnumerable<Card> hand)
        {
            return _calculator.CountShowPoints(card, hand).Points.Score;
        }

        public bool Equals(TestPlayer other)
        {
            return other?.Id == Id;
        }
    }
}