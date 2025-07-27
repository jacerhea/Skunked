using System;
using FluentAssertions;
using Skunked.Cards;
using Skunked.Domain.Commands;
using Skunked.Domain.State;
using Skunked.Domain.Validations;
using Skunked.Exceptions;
using Skunked.Rules;
using Xunit;

namespace Skunked.UnitTest.State.Validations;

public class HandCountedEventValidationTests
{
    [Fact]
    public void Card_Played_With_Throw_Cards_Not_Complete_Should_Throw_Validation_Exception()
    {
        var state = new GameState
        {
            Id = Guid.NewGuid(),
            PlayerIds = [1, 2],
            GameRules = new GameRules(),
            TeamScores = [new() { Players = [1] }, new() { Players = [2] }],
            OpeningRound = new OpeningRound
            {
                CutCards = []
            },
            Rounds =
            [
                new()
                {
                    ThrowCardsComplete = true,
                    PlayedCardsComplete = true,
                    ShowScores = [new() { Player = 1 }],
                    Starter = new Card(Rank.Ten, Suit.Diamonds),
                    Hands =
                    [
                        new(1,
                        [
                            new(Rank.Ace, Suit.Clubs), new(Rank.Ace, Suit.Diamonds), new(Rank.Ace, Suit.Hearts),
                            new(Rank.Ace, Suit.Spades)
                        ])
                    ]
                }
            ]
        };

        var @event = new CountHandCommand(1, 300);
        var validation = new CountHandCommandValidation();
        Action validate = () => validation.Validate(state, @event);
        validate.Should().Throw<InvalidCribbageOperationException>()
            .And.Operation.Should().Be(InvalidCribbageOperation.InvalidShowCount);
    }
}