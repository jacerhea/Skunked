using System;
using System.Collections.Generic;
using FluentAssertions;
using Skunked.Exceptions;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.State;
using Skunked.State.Events;
using Skunked.State.Validations;
using Xunit;

namespace Skunked.Standard.UnitTest.State.Validations
{
    public class CardCutEventValidationTests
    {
        [Fact]
        public void Cut_Card_With_No_Prior_Cuts_Should_Not_Throw_Validation_Exception()
        {
            var state = new GameState
            {
                PlayerIds = new List<int> { 1, 2 },
                GameRules = new GameRules(),
                TeamScores = new List<TeamScore>
                    {new TeamScore {Players = new List<int> {1}}, new TeamScore {Players = new List<int> {2}},},
                OpeningRound = new OpeningRound
                {
                    CutCards = new List<PlayerIdCard> { },
                }
            };

            var @event = new CardCutEvent { CutCard = new Card(Rank.Eight, Suit.Clubs), PlayerId = 1 };
            var validation = new CardCutEventValidation();
            Action validate = () => validation.Validate(state, @event);
            validate.Should().NotThrow();
        }

        [Fact]
        public void Cut_Card_Already_Cut_Should_Throw_Validation_Exception()
        {
            var state = new GameState
            {
                PlayerIds = new List<int> { 1, 2 },
                GameRules = new GameRules(),
                TeamScores = new List<TeamScore>
                    {new TeamScore {Players = new List<int> {1}}, new TeamScore {Players = new List<int> {2}},},
                OpeningRound = new OpeningRound
                {
                    CutCards = new List<PlayerIdCard> { new PlayerIdCard { Card = new Card(Rank.Eight, Suit.Diamonds), Player = 1 } },
                }
            };

            var @event = new CardCutEvent { CutCard = new Card(Rank.Eight, Suit.Diamonds), PlayerId = 2 };
            var validation = new CardCutEventValidation();
            Action validate = () => validation.Validate(state, @event);
            validate.Should().Throw<InvalidCribbageOperationException>("player 1 already cut the eight of diamonds");
        }

        [Fact]
        public void Player_Cutting_From_Deck_Twice_Should_Throw_Validation_Exception()
        {
            var state = new GameState
            {
                PlayerIds = new List<int> { 1, 2 },
                GameRules = new GameRules(),
                TeamScores = new List<TeamScore>
                    {new TeamScore {Players = new List<int> {1}}, new TeamScore {Players = new List<int> {2}},},
                OpeningRound = new OpeningRound
                {
                    CutCards = new List<PlayerIdCard> { new PlayerIdCard { Card = new Card(Rank.Nine, Suit.Hearts), Player = 1 } },
                }
            };

            var @event = new CardCutEvent { CutCard = new Card(Rank.Eight, Suit.Diamonds), PlayerId = 1 };
            var validation = new CardCutEventValidation();
            Action validate = () => validation.Validate(state, @event);
            validate.Should().Throw<InvalidCribbageOperationException>("player 1 already cut the eight of diamonds");
        }
    }
}
