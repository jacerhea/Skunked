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
                    Starter = new Card(Rank.King, Suit.Spades),
                    Crib = [new(Rank.Ace, Suit.Clubs), new(Rank.Two, Suit.Clubs), new(Rank.Three, Suit.Clubs), new(Rank.Four, Suit.Clubs)],
                    ThrowCardsComplete = throwComplete,
                    PlayedCardsComplete = playComplete,
                    Complete = roundComplete,
                    ShowScores =
                    [
                        new() { Player = 1, HasShowed = true },
                        new() { Player = 2, HasShowed = otherPlayerHasShowed, Complete = otherPlayerHasShowed }
                    ]
                }
            ]
        };

    [Fact]
    public void Validate_Succeeds_When_All_Non_Crib_Players_Have_Showed()
    {
        var state = BuildState(otherPlayerHasShowed: true);
        var command = new CountCribCommand(1, 0);
        var validation = new CountCribCommandValidation();

        Action act = () => validation.Validate(state, command);

        act();
    }

    [Fact]
    public void Validate_Throws_When_Round_Is_Already_Complete()
    {
        var state = BuildState(roundComplete: true);
        var command = new CountCribCommand(1, 4);
        var validation = new CountCribCommandValidation();

        Action act = () => validation.Validate(state, command);

        Assert.Equal(InvalidCribbageOperation.InvalidStateForCribCount, Assert.ThrowsAny<InvalidCribbageOperationException>(act).Operation);
    }

    [Fact]
    public void Validate_Throws_When_Throw_Phase_Not_Complete()
    {
        var state = BuildState(throwComplete: false);
        var command = new CountCribCommand(1, 4);
        var validation = new CountCribCommandValidation();

        Action act = () => validation.Validate(state, command);

        Assert.Equal(InvalidCribbageOperation.InvalidStateForCribCount, Assert.ThrowsAny<InvalidCribbageOperationException>(act).Operation);
    }

    [Fact]
    public void Validate_Throws_When_Play_Phase_Not_Complete()
    {
        var state = BuildState(playComplete: false);
        var command = new CountCribCommand(1, 4);
        var validation = new CountCribCommandValidation();

        Action act = () => validation.Validate(state, command);

        Assert.Equal(InvalidCribbageOperation.InvalidStateForCribCount, Assert.ThrowsAny<InvalidCribbageOperationException>(act).Operation);
    }

    [Fact]
    public void Validate_Throws_When_Other_Players_Have_Not_Showed()
    {
        var state = BuildState(otherPlayerHasShowed: false);
        var command = new CountCribCommand(1, 4);
        var validation = new CountCribCommandValidation();

        Action act = () => validation.Validate(state, command);

        Assert.Equal(InvalidCribbageOperation.NotPlayersTurn, Assert.ThrowsAny<InvalidCribbageOperationException>(act).Operation);
    }
    [Fact] public void Validate_Rejects_NonDealer()
    {
        var state = BuildState();
        Action act = () => new CountCribCommandValidation().Validate(state, new CountCribCommand(2, 0));
        Assert.Equal(InvalidCribbageOperation.NotPlayersTurn, Assert.ThrowsAny<InvalidCribbageOperationException>(act).Operation);
    }
    [Theory] [InlineData(-1)] [InlineData(99)]
    public void Validate_Rejects_Invalid_Declared_Score(int score)
    {
        var state = BuildState();
        Action act = () => new CountCribCommandValidation().Validate(state, new CountCribCommand(1, score));
        Assert.Equal(InvalidCribbageOperation.InvalidShowCount, Assert.ThrowsAny<InvalidCribbageOperationException>(act).Operation);
    }
}
