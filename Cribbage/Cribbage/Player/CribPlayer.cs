using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage.Rules;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.AI.CardToss;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.AI.TheCount;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.AI.ThePlay;
using Games.Domain.MainModule.Entities.CardGames.Player;
using Games.Domain.MainModule.Entities.PlayingCards;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.Player
{
    public class CribPlayer : PlayerBase, ICribPlayer
    {
        private readonly IPlayStrategy _playStrategy;
        private readonly IDecisionStrategy _decisionStrategy;
        private readonly IScoreCountStrategy _scoreCountStrategy;

        public CribPlayer(string name, IPlayStrategy playStrategy, IDecisionStrategy decisionStrategy, IScoreCountStrategy scoreCountStrategy) : base(name)
        {
            if (playStrategy == null) throw new ArgumentNullException("playStrategy");
            if (decisionStrategy == null) throw new ArgumentNullException("decisionStrategy");
            if (scoreCountStrategy == null) throw new ArgumentNullException("scoreCountStrategy");
            _playStrategy = playStrategy;
            _decisionStrategy = decisionStrategy;
            _scoreCountStrategy = scoreCountStrategy;
        }

        public CribPlayer(string name, int id, IPlayStrategy playStrategy, IDecisionStrategy decisionStrategy)
            : base(name, id)
        {
            if (playStrategy == null) throw new ArgumentNullException("playStrategy");
            if (decisionStrategy == null) throw new ArgumentNullException("decisionStrategy");
            _playStrategy = playStrategy;
            _decisionStrategy = decisionStrategy;
        }

        /// <summary>
        /// Deal Hand and return cards that will go back in crib
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>Set of Cards to throw in crib.</returns>
        public List<ICard> DealHand(IList<ICard> hand)
        {
            return _decisionStrategy.DetermineCardsToThrow(hand).ToList();
        }

        public ICard PlayShow(CribGameRules gameRules, List<ICard> pile, List<ICard> handLeft)
        {
            if (gameRules == null) throw new ArgumentNullException("gameRules");
            if (pile == null) throw new ArgumentNullException("pile");
            if (handLeft == null) throw new ArgumentNullException("handLeft");
            if(handLeft.Count == 0) throw new ArgumentException("handLeft");

            return _playStrategy.DetermineCardToThrow(gameRules, pile, handLeft);
        }

        public ICard ChooseCard(List<ICard> cardsToChoose)
        {
            if (cardsToChoose == null) throw new ArgumentNullException("cardsToChoose");
            var randomGenerator = new Random();
            var randomIndex = randomGenerator.Next(0 ,cardsToChoose.Count);
            return cardsToChoose[randomIndex];
        }

        public int CountHand(ICard card, IEnumerable<ICard> hand)
        {
            return _scoreCountStrategy.GetCount(card, hand);
        }

        public bool Equals(ICribPlayer other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return other.ID == ID;
        }
    }
}