using System.Collections.Generic;
using System.Linq;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.AI.Play
{
    public class MaxDifferencePlayStrategy : BasePlay, IPlayStrategy
    {
        private readonly ScoreCalculator _scoreCalculator;
        private readonly ICardValueStrategy _valueStrategy;


        public MaxDifferencePlayStrategy(ScoreCalculator scoreCalculator = null, ICardValueStrategy valueStrategy = null)
        {
            _scoreCalculator = scoreCalculator ?? new ScoreCalculator();
            _valueStrategy = valueStrategy ?? new AceLowFaceTenCardValueStrategy();
        }

        public Card DetermineCardToThrow(GameRules gameRules, IList<Card> pile, IEnumerable<Card> handLeft)
        {
            ArgumentCheck(pile, handLeft);
            handLeft = handLeft.ToList();
            int currentPileCount = _scoreCalculator.SumValues(pile);
            var validPlays = handLeft.Where(c => IsValid(currentPileCount, c)).ToList();

            var x = GetRemainingPossibilities(new PlayState
            {
                Pile = pile.ToList(),
                HandLeft = handLeft.ToList(),
                Count = _scoreCalculator.SumValues(pile),
                MyTurn = true
            })
                .ToList();

            return validPlays.First();
        }


        //public for testing purposes.
        public IEnumerable<List<Card>> GetRemainingPossibilities(PlayState state)
        {
            var possibilitySet  = new List<List<Card>>();

            if (state.MyTurn)
            {
                foreach (var card in state.HandLeft.Where(c => state.MyTurn && IsValid(state.Count, c)))
                {
                    var newPile = state.Pile.Append(card).ToList();
                    var newcardsLeft = state.HandLeft.Except(new List<Card> { card }).ToList();
                    var playState = new PlayState
                    {
                        Pile = newPile,
                        HandLeft = newcardsLeft,
                        MyTurn = false,
                        Count = state.Count + _valueStrategy.ValueOf(card)
                    };

                    if (IsRoundDone(playState))
                    {
                        possibilitySet.Add(newPile);
                    }
                    else
                    {
                        var remainingPossibilites = GetRemainingPossibilities(playState);
                        possibilitySet.AddRange(remainingPossibilites);
                    }
                }
            }
            else
            {
                var opponentPossibilities = state.RemainingCards.Where(card => IsValid(state.Count, card)).ToList();

                foreach (var opponentCard in opponentPossibilities)
                {
                    var newPile = state.Pile.Append(opponentCard).ToList();
                    var newHandLeft = state.HandLeft.Except(new List<Card> { opponentCard }).ToList();
                    var playState = new PlayState
                    {
                        Pile = newPile,
                        HandLeft = newHandLeft,
                        MyTurn = true,
                        Count = state.Count + _valueStrategy.ValueOf(opponentCard)
                    };

                    if (IsRoundDone(playState))
                    {
                        possibilitySet.Add(newPile);
                    }
                    else
                    {
                        var remainingPossibilites = GetRemainingPossibilities(playState);
                        possibilitySet.AddRange(remainingPossibilites);
                    }
                }
            }

            return possibilitySet;
        }


        private bool IsRoundDone(PlayState state)
        {
            return !state.HandLeft.Any(c => IsValid(state.Count, c)) || !state.FullDeck.Any(card => IsValid(state.Count, card));
        }


        private bool IsValid(int currentPileCount, Card c)
        {
            return currentPileCount + _valueStrategy.ValueOf(c) <= GameRules.PlayMaxScore;
        }

        public class PlayState
        {
            public Deck FullDeck => new Deck();
            public int Count { get; set; }

            public List<Card> Pile { get; set; }
            public List<Card> HandLeft { get; set; }
            public bool MyTurn { get; set; }
            public IEnumerable<Card> RemainingCards => FullDeck.Except(PlayedCards);
            public IEnumerable<Card> PlayedCards => Pile.Concat(HandLeft);
        }

        public class PlayResult
        {
            public List<Card> Pile { get; set; }
            public List<Card> HandLeft { get; set; }
            public int PileCount { get; set; }
            public int MyScore { get; set; }
        }
    }
}
