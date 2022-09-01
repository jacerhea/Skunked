using FluentAssertions;
using Skunked.Cards;
using Skunked.Domain.Commands;
using Skunked.Domain.State;
using Skunked.Domain.Validations;
using Skunked.Exceptions;
using Skunked.Players;
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
                    ShowScores = new List<PlayerScoreShow>{new() { Player = 1} },
                    Starter = new Card(Rank.Ten, Suit.Diamonds),
                    Hands = new List<PlayerHand>{new(1, new List<Card>{new(Rank.Ace, Suit.Clubs), new(Rank.Ace, Suit.Diamonds), new(Rank.Ace, Suit.Hearts), new(Rank.Ace, Suit.Spades)}) }
                }
            }
        };

        var @event = new CountHandCommand(1, 300);
        var validation = new CountHandCommandValidation();
        Action validate = () => validation.Validate(state, @event);
        validate.Should().Throw<InvalidCribbageOperationException>()
            .And.Operation.Should().Be(InvalidCribbageOperation.InvalidShowCount);
    }
}