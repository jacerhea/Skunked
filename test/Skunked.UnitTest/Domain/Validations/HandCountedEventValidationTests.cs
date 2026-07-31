using FluentAssertions;
using Xunit;

namespace Skunked.UnitTest.Domain.Validations;

public class HandCountedEventValidationTests
{
    // Player 1 is dealer; player 2 (non-dealer) shows first.
    private static GameState BuildState(
        bool playComplete = true,
        bool throwComplete = true,
        bool player2HasShowed = false) =>
        new()
        {
            Id = Guid.NewGuid(),
            PlayerIds = [1, 2],
            GameRules = new GameRules(),
            TeamScores = [new() { Players = [1] }, new() { Players = [2] }],
            OpeningRound = new OpeningRound { CutCards = [] },
            Rounds =
            [
                new()
                {
                    Round = 1,
                    PlayerCrib = 1,
                    ThrowCardsComplete = throwComplete,
                    PlayedCardsComplete = playComplete,
                    Starter = new Card(Rank.Ten, Suit.Diamonds),
                    Hands =
                    [
                        new(1, [
                            new(Rank.Ace, Suit.Clubs), new(Rank.Ace, Suit.Diamonds),
                            new(Rank.Ace, Suit.Hearts), new(Rank.Ace, Suit.Spades)
                        ]),

                        new(2, [
                            new(Rank.Two, Suit.Clubs), new(Rank.Three, Suit.Clubs),
                            new(Rank.Four, Suit.Clubs), new(Rank.Five, Suit.Clubs)
                        ])
                    ],
                    ShowScores =
                    [
                        new() { Player = 1, HasShowed = false },
                        new() { Player = 2, HasShowed = player2HasShowed, Complete = player2HasShowed }
                    ]
                }
            ]
        };

    [Fact]
    public void Validate_Throws_When_Submitted_Score_Exceeds_Maximum_Possible()
    {
        // Player 2 has showed, now player 1 (dealer) submits an impossibly high score
        var state = BuildState(player2HasShowed: true);
        var command = new CountHandCommand(1, 300);
        var validation = new CountHandCommandValidation();

        Action act = () => validation.Validate(state, command);

        act.Should().Throw<InvalidCribbageOperationException>()
            .And.Operation.Should().Be(InvalidCribbageOperation.InvalidShowCount);
    }

    [Fact]
    public void Validate_Throws_When_Play_Phase_Not_Complete()
    {
        var state = BuildState(playComplete: false);
        var command = new CountHandCommand(2, 0);
        var validation = new CountHandCommandValidation();

        Action act = () => validation.Validate(state, command);

        act.Should().Throw<InvalidCribbageOperationException>()
            .And.Operation.Should().Be(InvalidCribbageOperation.InvalidStateForCount);
    }

    [Fact]
    public void Validate_Throws_When_Player_Has_Already_Counted()
    {
        // Player 2 already showed; tries to count again
        var state = BuildState(player2HasShowed: true);
        var command = new CountHandCommand(2, 0);
        var validation = new CountHandCommandValidation();

        Action act = () => validation.Validate(state, command);

        act.Should().Throw<InvalidCribbageOperationException>()
            .And.Operation.Should().Be(InvalidCribbageOperation.PlayerHasAlreadyCounted);
    }

    [Fact]
    public void Validate_Throws_When_Not_Players_Turn()
    {
        // Player 2 has NOT shown yet; player 1 (dealer) tries to count first — out of order
        var state = BuildState(player2HasShowed: false);
        var command = new CountHandCommand(1, 0);
        var validation = new CountHandCommandValidation();

        Action act = () => validation.Validate(state, command);

        act.Should().Throw<InvalidCribbageOperationException>()
            .And.Operation.Should().Be(InvalidCribbageOperation.NotPlayersTurn);
    }

    [Fact]
    public void Validate_Succeeds_When_First_Shower_Submits_Correct_Score()
    {
        var state = BuildState(player2HasShowed: false);
        var scorer = new ScoreCalculator();
        var round = state.GetCurrentRound();
        var hand = round.Hands.Single(h => h.PlayerId == 2).Hand;
        var score = scorer.CountShowPoints(round.Starter, hand).Points.Score;
        var command = new CountHandCommand(2, score);
        var validation = new CountHandCommandValidation();

        Action act = () => validation.Validate(state, command);

        act.Should().NotThrow();
    }
}
