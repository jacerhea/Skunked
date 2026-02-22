using FluentAssertions;
using Xunit;

namespace Skunked.UnitTest.Domain.Validations;

public sealed class CountCribCommandValidationTests
{
    // Player 1 is the dealer/crib holder; player 2 shows first.
    private static GameState BuildState(
        bool throwComplete = true,
        bool playComplete = true,
        bool roundComplete = false,
        bool otherPlayerHasShowed = true) =>
        new()
        {
            Id = Guid.NewGuid(),
            PlayerIds = [1, 2],
            GameRules = new GameRules(),
            TeamScores =
            [
                new() { Players = [1] },
                new() { Players = [2] }
            ],
            OpeningRound = new OpeningRound { CutCards = [] },
            Rounds =
            [
                new()
                {
                    Round = 1,
                    PlayerCrib = 1,
                    ThrowCardsComplete = throwComplete,
                    PlayedCardsComplete = playComplete,
                    Complete = roundComplete,
                    ShowScores =
                    [
                        new() { Player = 1, HasShowed = false },
                        new() { Player = 2, HasShowed = otherPlayerHasShowed, Complete = otherPlayerHasShowed }
                    ]
                }
            ]
        };

    [Fact]
    public void Validate_Succeeds_When_All_Non_Crib_Players_Have_Showed()
    {
        var state = BuildState(otherPlayerHasShowed: true);
        var command = new CountCribCommand(1, 4);
        var validation = new CountCribCommandValidation();

        Action act = () => validation.Validate(state, command);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_Throws_When_Round_Is_Already_Complete()
    {
        var state = BuildState(roundComplete: true);
        var command = new CountCribCommand(1, 4);
        var validation = new CountCribCommandValidation();

        Action act = () => validation.Validate(state, command);

        act.Should().Throw<InvalidCribbageOperationException>()
            .And.Operation.Should().Be(InvalidCribbageOperation.InvalidStateForCribCount);
    }

    [Fact]
    public void Validate_Throws_When_Throw_Phase_Not_Complete()
    {
        var state = BuildState(throwComplete: false);
        var command = new CountCribCommand(1, 4);
        var validation = new CountCribCommandValidation();

        Action act = () => validation.Validate(state, command);

        act.Should().Throw<InvalidCribbageOperationException>()
            .And.Operation.Should().Be(InvalidCribbageOperation.InvalidStateForCribCount);
    }

    [Fact]
    public void Validate_Throws_When_Play_Phase_Not_Complete()
    {
        var state = BuildState(playComplete: false);
        var command = new CountCribCommand(1, 4);
        var validation = new CountCribCommandValidation();

        Action act = () => validation.Validate(state, command);

        act.Should().Throw<InvalidCribbageOperationException>()
            .And.Operation.Should().Be(InvalidCribbageOperation.InvalidStateForCribCount);
    }

    [Fact]
    public void Validate_Throws_When_Other_Players_Have_Not_Showed()
    {
        var state = BuildState(otherPlayerHasShowed: false);
        var command = new CountCribCommand(1, 4);
        var validation = new CountCribCommandValidation();

        Action act = () => validation.Validate(state, command);

        act.Should().Throw<InvalidCribbageOperationException>()
            .And.Operation.Should().Be(InvalidCribbageOperation.NotPlayersTurn);
    }
}
