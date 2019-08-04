using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Skunked.Exceptions;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Score;
using Skunked.State;
using Skunked.State.Events;
using Skunked.State.Validations;
using Xunit;

namespace Skunked.UnitTest.State.Validations
{
    public class HandCountedEventValidationTests
    {
        [Fact]
        public void Card_Played_With_Throw_Cards_Not_Complete_Should_Throw_Validation_Exception()
        {
            var state = new GameState
            {
                Id = Guid.NewGuid(),
                PlayerIds = new List<int> { 1, 2 },
                GameRules = new GameRules(),
                TeamScores = new List<TeamScore>
                    {new TeamScore {Players = new List<int> {1}}, new TeamScore {Players = new List<int> {2}}},
                OpeningRound = new OpeningRound
                {
                    CutCards = new List<PlayerIdCard>()
                },
                Rounds = new List<RoundState>
                {
                    new RoundState
                    {
                        ThrowCardsComplete = true,
                        PlayedCardsComplete = true,
                        ShowScores = new List<PlayerScoreShow>{new PlayerScoreShow { Player = 1} },
                        Starter = new Card(Rank.Ten, Suit.Diamonds),
                        Hands = new List<PlayerHand>{new PlayerHand(1, new List<Card>{new Card(Rank.Ace, Suit.Clubs), new Card(Rank.Ace, Suit.Diamonds), new Card(Rank.Ace, Suit.Hearts), new Card(Rank.Ace, Suit.Spades)}) }
                    }
                }
            };

            var @event = new HandCountedEvent { GameId = state.Id, PlayerId = 1, CountedScore = 300 };
            var validation = new HandCountedEventValidation(new ScoreCalculator());
            Action validate = () => validation.Validate(state, @event);
            validate.Should().Throw<InvalidCribbageOperationException>()
                .And.Operation.Should().Be(InvalidCribbageOperation.InvalidShowCount);
        }
    }
}
