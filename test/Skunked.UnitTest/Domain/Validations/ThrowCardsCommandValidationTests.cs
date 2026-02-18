using FluentAssertions;
using Skunked;
using Xunit;

namespace Skunked.UnitTest.State.Validations;

public sealed class ThrowCardsCommandValidationTests
{
    private static GameState BuildState(bool throwComplete = false) =>
        new()
        {
            Id = Guid.NewGuid(),
            PlayerIds = new List<int> { 1, 2 },
            GameRules = new GameRules(),
            TeamScores = new List<TeamScore>
            {
                new() { Players = new List<int> { 1 } },
                new() { Players = new List<int> { 2 } }
            },
            OpeningRound = new OpeningRound { CutCards = new List<PlayerIdCard>() },
            Rounds = new List<RoundState>
            {
                new()
                {
                    Round = 1,
                    ThrowCardsComplete = throwComplete,
                    Crib = new List<Card>(),
                    DealtCards = new List<PlayerHand>
                    {
                        new(1, new List<Card>
                        {
                            new(Rank.Ace, Suit.Clubs), new(Rank.Two, Suit.Clubs),
                            new(Rank.Three, Suit.Clubs), new(Rank.Four, Suit.Clubs),
                            new(Rank.Five, Suit.Clubs), new(Rank.Six, Suit.Clubs)
                        }),
                        new(2, new List<Card>
                        {
                            new(Rank.Seven, Suit.Hearts), new(Rank.Eight, Suit.Hearts),
                            new(Rank.Nine, Suit.Hearts), new(Rank.Ten, Suit.Hearts),
                            new(Rank.Jack, Suit.Hearts), new(Rank.Queen, Suit.Hearts)
                        })
                    }
                }
            }
        };

    [Fact]
    public void Validate_Succeeds_When_Player_Throws_Cards_From_Their_Hand()
    {
        var state = BuildState();
        var command = new ThrowCardsCommand(1, new List<Card>
        {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.Two, Suit.Clubs)
        });
        var validation = new ThrowCardsCommandValidation();

        Action act = () => validation.Validate(state, command);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_Throws_When_Cards_Have_Already_Been_Thrown()
    {
        var state = BuildState(throwComplete: true);
        var command = new ThrowCardsCommand(1, new List<Card>
        {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.Two, Suit.Clubs)
        });
        var validation = new ThrowCardsCommandValidation();

        Action act = () => validation.Validate(state, command);

        act.Should().Throw<InvalidCribbageOperationException>()
            .And.Operation.Should().Be(InvalidCribbageOperation.CardsHaveBeenThrown);
    }

    [Fact]
    public void Validate_Throws_When_Player_Submits_Cards_Not_In_Their_Dealt_Hand()
    {
        var state = BuildState();
        var command = new ThrowCardsCommand(1, new List<Card>
        {
            new(Rank.King, Suit.Spades),  // not dealt to player 1
            new(Rank.Two, Suit.Clubs)
        });
        var validation = new ThrowCardsCommandValidation();

        Action act = () => validation.Validate(state, command);

        act.Should().Throw<InvalidCribbageOperationException>()
            .And.Operation.Should().Be(InvalidCribbageOperation.InvalidCard);
    }
}
