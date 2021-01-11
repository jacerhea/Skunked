using System;
using System.Collections.Generic;
using System.Threading;
using Skunked.Cards;
using Skunked.Rules;
using Skunked.Utility;
using Xunit;

namespace Skunked.Test.System
{
    public class FullGameTest
    {
        public FullGameTest()
        {
            //RandomProvider.RandomInstance = new ThreadLocal<Random>(() => new IncrementalRandom());
        }

        [Fact]
        public void Controlled_Play_Create_Correct_Outcome()
        {
            var players = new List<int> { 1, 2, 3, 4 };

            var game = new Cribbage(players, new GameRules(WinningScoreType.Short61, 4));

            //cut cards for first deal.
            game.CutCard(1, new Card(Rank.Eight, Suit.Diamonds));
            game.CutCard(2, new Card(Rank.Five, Suit.Clubs));
            game.CutCard(3, new Card(Rank.King, Suit.Spades));
            game.CutCard(4, new Card(Rank.Ace, Suit.Hearts));

            //player 4 won, deals first.
            game.ThrowCards(1, new List<Card> { new(Rank.Nine, Suit.Diamonds) });
            game.ThrowCards(2, new List<Card> { new(Rank.Ten, Suit.Hearts) });
            game.ThrowCards(3, new List<Card> { new(Rank.King, Suit.Clubs) });
            game.ThrowCards(4, new List<Card> { new(Rank.Jack, Suit.Clubs) });

            //player 1, play 1
            game.PlayCard(1, new Card(Rank.King, Suit.Hearts));
            game.PlayCard(2, new Card(Rank.Nine, Suit.Spades));
            game.PlayCard(3, new Card(Rank.Nine, Suit.Hearts));

            game.PlayCard(4, new Card(Rank.Nine, Suit.Clubs));
            game.PlayCard(1, new Card(Rank.King, Suit.Spades));
            game.PlayCard(2, new Card(Rank.Eight, Suit.Spades));
            game.PlayCard(3, new Card(Rank.Eight, Suit.Hearts));
            game.PlayCard(4, new Card(Rank.Nine, Suit.Diamonds));
            game.PlayCard(1, new Card(Rank.Nine, Suit.Diamonds));
            game.PlayCard(2, new Card(Rank.Nine, Suit.Diamonds));
            game.PlayCard(3, new Card(Rank.Nine, Suit.Diamonds));
            game.PlayCard(4, new Card(Rank.Nine, Suit.Diamonds));
            game.PlayCard(1, new Card(Rank.Nine, Suit.Diamonds));
            game.PlayCard(2, new Card(Rank.Nine, Suit.Diamonds));
            game.PlayCard(3, new Card(Rank.Nine, Suit.Diamonds));
            game.PlayCard(4, new Card(Rank.Nine, Suit.Diamonds));
        }
    }
}
