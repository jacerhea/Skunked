using System;
using System.Collections.Generic;
using FluentAssertions;
using Skunked.Cards;
using Skunked.Domain.Commands;
using Skunked.Domain.State;
using Skunked.Domain.Validations;
using Skunked.Exceptions;
using Skunked.Players;
using Skunked.Rules;
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
                    {new() {Players = new List<int> {1}}, new() {Players = new List<int> {2}}},
                OpeningRound = new OpeningRound
                {
                    CutCards = new List<PlayerIdCard>()
                },
                Rounds = new List<RoundState>
                {
                    new()
                    {
                        ThrowCardsComplete = false
                    }
                }
            };

            var command = new PlayCardCommand(1, new Card(Rank.Eight, Suit.Clubs));
            var validation = new PlayCardCommandValidation();
            Action validate = () => validation.Validate(state, command);
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
                    {new() {Players = new List<int> {1}}, new() {Players = new List<int> {2}}},
                OpeningRound = new OpeningRound
                {
                    CutCards = new List<PlayerIdCard>()
                },
                Rounds = new List<RoundState>
                {
                    new()
                    {
                        ThrowCardsComplete = true,
                        PlayedCardsComplete = true,
                        Hands = new List<PlayerHand>
                        {
                            new(1, new List<Card>())
                        }
                    }
                }
            };

            var command = new PlayCardCommand(1, new Card(Rank.Eight, Suit.Clubs));

            var validation = new PlayCardCommandValidation();
            Action validate = () => validation.Validate(state, command);
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
                    {new() {Players = new List<int> {1}}, new() {Players = new List<int> {2}}},
                OpeningRound = new OpeningRound
                {
                    CutCards = new List<PlayerIdCard>()
                },
                Rounds = new List<RoundState>
                {
                    new()
                    {
                        ThrowCardsComplete = true,
                        PlayedCardsComplete = false,
                        Hands = new List<PlayerHand>
                        {
                            new(1, new List<Card>{new(Rank.Five, Suit.Clubs), new(Rank.Eight, Suit.Clubs)}),
                            new(2, new List<Card>{new(Rank.Seven, Suit.Hearts), new(Rank.Nine, Suit.Diamonds)})
                        }
                    }
                }
            };

            var command = new PlayCardCommand(1, new Card(Rank.King, Suit.Diamonds));

            var validation = new PlayCardCommandValidation();
            Action validate = () => validation.Validate(state, command);
            validate.Should().Throw<InvalidCribbageOperationException>()
                .And.Operation.Should().Be(InvalidCribbageOperation.InvalidCard);
        }
    }
}
