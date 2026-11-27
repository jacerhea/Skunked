using Xunit;

namespace Skunked.UnitTest.Domain.Validations;

public class CardCutEventValidationTests
{
    [Fact]
    public void Cut_Card_With_No_Prior_Cuts_Should_Not_Throw_Validation_Exception()
    {
        var state = new GameState
        {
            PlayerIds = [1, 2],
            GameRules = new GameRules(),
            TeamScores = [new() { Players = [1] }, new() { Players = [2] }],
            OpeningRound = new OpeningRound
            {
                Deck = new Deck().ToList(),
                CutCards = []
            }
        };

        var command = new CutCardCommand(1, new Card(Rank.Eight, Suit.Clubs));
        var validation = new CutCardCommandValidation();
        Action validate = () => validation.Validate(state, command);
        validate();
    }

    [Fact]
    public void Cut_Card_Already_Cut_Should_Throw_Validation_Exception()
    {
        var state = new GameState
        {
            PlayerIds = [1, 2],
            GameRules = new GameRules(),
            TeamScores = [new() { Players = [1] }, new() { Players = [2] }],
            OpeningRound = new OpeningRound
            {
                Deck = new Deck().ToList(),
                CutCards = [new(1, new Card(Rank.Eight, Suit.Diamonds))]
            }
        };

        var command = new CutCardCommand(2, new Card(Rank.Eight, Suit.Diamonds));
        var validation = new CutCardCommandValidation();
        Action validate = () => validation.Validate(state, command);
        Assert.ThrowsAny<InvalidCribbageOperationException>(validate);
    }

    [Fact]
    public void Player_Cutting_From_Deck_Twice_Should_Throw_Validation_Exception()
    {
        var state = new GameState
        {
            PlayerIds = [1, 2],
            GameRules = new GameRules(),
            TeamScores = [new() { Players = [1] }, new() { Players = [2] }],
            OpeningRound = new OpeningRound
            {
                Deck = new Deck().ToList(),
                CutCards = [new(1, new Card(Rank.Nine, Suit.Hearts))]
            }
        };

        var command = new CutCardCommand(1, new Card(Rank.Eight, Suit.Diamonds));

        var validation = new CutCardCommandValidation();
        Action validate = () => validation.Validate(state, command);
        Assert.ThrowsAny<InvalidCribbageOperationException>(validate);
    }
}