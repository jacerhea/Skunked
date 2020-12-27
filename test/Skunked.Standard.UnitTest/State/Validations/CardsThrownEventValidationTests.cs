using System;
using System.Collections.Generic;
using FluentAssertions;
using Skunked.Cards;
using Skunked.Exceptions;
using Skunked.Players;
using Skunked.Rules;
using Skunked.State;
using Skunked.State.Events;
using Skunked.State.Validations;
using Xunit;

namespace Skunked.UnitTest.State.Validations
{
    public class CardsThrownEventValidationTests
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
                        ThrowCardsComplete = false
                    }
                }
            };

            var @event = new CardPlayedEvent { PlayerId = 1, GameId = state.Id, Played = new Card(Rank.Eight, Suit.Clubs) };
            var validation = new CardPlayedEventValidation();
            Action validate = () => validation.Validate(state, @event);
            validate.Should().Throw<InvalidCribbageOperationException>()
                .And.Operation.Should().Be(InvalidCribbageOperation.InvalidStateForPlay);
        }

        [Fact]
        public void Card_Played_With_PlayedCardsComplete_Should_Throw_Validation_Exception()
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
                        Hands = new List<PlayerHand>
                        {
                            new PlayerHand(1, new List<Card>())
                        }
                    }
                }
            };

            var @event = new CardPlayedEvent { PlayerId = 1, GameId = state.Id, Played = new Card(Rank.Eight, Suit.Clubs) };
            var validation = new CardPlayedEventValidation();
            Action validate = () => validation.Validate(state, @event);
            validate.Should().Throw<InvalidCribbageOperationException>()
                .And.Operation.Should().Be(InvalidCribbageOperation.InvalidStateForPlay);
        }

        [Fact]
        public void Card_Played_That_Player_Does_Not_Have_Should_Throw_Exception()
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
                        PlayedCardsComplete = false,
                        Hands = new List<PlayerHand>
                        {
                            new PlayerHand(1, new List<Card>{new Card(Rank.Five, Suit.Clubs), new Card(Rank.Eight, Suit.Clubs)}),
                            new PlayerHand(2, new List<Card>{new Card(Rank.Seven, Suit.Hearts), new Card(Rank.Nine, Suit.Diamonds)})
                        }
                    }
                }
            };

            var @event = new CardPlayedEvent { PlayerId = 1, GameId = state.Id, Played = new Card(Rank.King, Suit.Diamonds) };
            var validation = new CardPlayedEventValidation();
            Action validate = () => validation.Validate(state, @event);
            validate.Should().Throw<InvalidCribbageOperationException>()
                .And.Operation.Should().Be(InvalidCribbageOperation.InvalidCard);
        }
    }
}
