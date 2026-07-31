using FluentAssertions;
using Xunit;

namespace Skunked.UnitTest.Domain.Validations;

public class CardPlayedEventValidationTests
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
                    ThrowCardsComplete = false
                }
            ]
        };

        var @event = new PlayCardCommand(1, new Card(Rank.Eight, Suit.Clubs));
        var validation = new PlayCardCommandValidation();
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
                    Hands = [new(1, [])]
                }
            ]
        };

        var @event = new PlayCardCommand(1, new Card(Rank.Eight, Suit.Clubs));
        var validation = new PlayCardCommandValidation();
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
                    PlayedCardsComplete = false,
                    Hands =
                    [
                        new(1, [new(Rank.Five, Suit.Clubs), new(Rank.Eight, Suit.Clubs)]),
                        new(2, [new(Rank.Seven, Suit.Hearts), new(Rank.Nine, Suit.Diamonds)])
                    ]
                }
            ]
        };

        var @event = new PlayCardCommand(1, new Card(Rank.King, Suit.Diamonds));
        var validation = new PlayCardCommandValidation();
        Action validate = () => validation.Validate(state, @event);
        validate.Should().Throw<InvalidCribbageOperationException>()
            .And.Operation.Should().Be(InvalidCribbageOperation.InvalidCard);
    }
}