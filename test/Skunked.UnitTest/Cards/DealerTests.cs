using FluentAssertions;
using Xunit;

namespace Skunked.UnitTest.Cards;

public class DealerTests

{
    [Fact]
    public void Two_Player_Deal_Starting_With_Player1_Will_Be_Correct()
    {
        var deck = new Deck(new List<Card>
        {
            new(Rank.King, Suit.Clubs),
            new(Rank.Ace, Suit.Diamonds),
            new(Rank.Eight, Suit.Hearts),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Diamonds),
            new(Rank.Jack, Suit.Hearts),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Queen, Suit.Spades),
            new(Rank.Seven, Suit.Clubs),
            new(Rank.Six, Suit.Hearts),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Three, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Three, Suit.Spades)
        });

        var handFactory = new Dealer();
        var players = new List<int> { 1, 2 };
        var hands = handFactory.Deal(deck, players, players[0], 6).ToDictionary(p => p.PlayerId, p => p.Hand);

        var player1Actual = hands[players[0]];
        player1Actual[0].Should().Be(new Card(Rank.King, Suit.Clubs));
        player1Actual[1].Should().Be(new Card(Rank.Eight, Suit.Hearts));
        player1Actual[2].Should().Be(new Card(Rank.Four, Suit.Diamonds));
        player1Actual[3].Should().Be(new Card(Rank.Nine, Suit.Diamonds));
        player1Actual[4].Should().Be(new Card(Rank.Seven, Suit.Clubs));
        player1Actual[5].Should().Be(new Card(Rank.Ten, Suit.Clubs));
        player1Actual.Count.Should().Be(6);

        var player2Actual = hands[players[1]];
        player2Actual[0].Should().Be(new Card(Rank.Ace, Suit.Diamonds));
        player2Actual[1].Should().Be(new Card(Rank.Five, Suit.Spades));
        player2Actual[2].Should().Be(new Card(Rank.Jack, Suit.Hearts));
        player2Actual[3].Should().Be(new Card(Rank.Queen, Suit.Spades));
        player2Actual[4].Should().Be(new Card(Rank.Six, Suit.Hearts));
        player2Actual[5].Should().Be(new Card(Rank.Three, Suit.Spades));
        player2Actual.Count.Should().Be(6);
    }

    [Fact]
    public void Two_Player_Deal_Starting_With_Player2_Will_Be_Correct()
    {
        var deck = new Deck(new List<Card>
        {
            new(Rank.King, Suit.Clubs),
            new(Rank.Ace, Suit.Diamonds),
            new(Rank.Eight, Suit.Hearts),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Diamonds),
            new(Rank.Jack, Suit.Hearts),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Queen, Suit.Spades),
            new(Rank.Seven, Suit.Clubs),
            new(Rank.Six, Suit.Hearts),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Three, Suit.Spades),
            new(Rank.Three, Suit.Spades)
        });

        var handFactory = new Dealer();
        var players = new List<int> { 1, 2 };
        var hands = handFactory.Deal(deck, players, players[1], 6).ToDictionary(p => p.PlayerId, p => p.Hand);


        var player2Actual = hands[players[1]];
        player2Actual[0].Should().Be(new Card(Rank.King, Suit.Clubs));
        player2Actual[1].Should().Be(new Card(Rank.Eight, Suit.Hearts));
        player2Actual[2].Should().Be(new Card(Rank.Four, Suit.Diamonds));
        player2Actual[3].Should().Be(new Card(Rank.Nine, Suit.Diamonds));
        player2Actual[4].Should().Be(new Card(Rank.Seven, Suit.Clubs));
        player2Actual[5].Should().Be(new Card(Rank.Ten, Suit.Clubs));
        player2Actual.Count.Should().Be(6);

        var player1Actual = hands[players[0]];
        player1Actual[0].Should().Be(new Card(Rank.Ace, Suit.Diamonds));
        player1Actual[1].Should().Be(new Card(Rank.Five, Suit.Spades));
        player1Actual[2].Should().Be(new Card(Rank.Jack, Suit.Hearts));
        player1Actual[3].Should().Be(new Card(Rank.Queen, Suit.Spades));
        player1Actual[4].Should().Be(new Card(Rank.Six, Suit.Hearts));
        player1Actual[5].Should().Be(new Card(Rank.Three, Suit.Spades));
        player1Actual.Count.Should().Be(6);
    }

    [Fact]
    public void Two_Player_Deal_Of_Zero_Cards_Will_Result_In_Empty_Hands()
    {
        var deck = new Deck();

        var handFactory = new Dealer();
        var players = new List<int> { 1, 2 };
        var hands = handFactory.Deal(deck, players, players[0], 0).ToDictionary(p => p.PlayerId, p => p.Hand);
        hands[players[0]].Count.Should().Be(0);
        hands[players[1]].Count.Should().Be(0);
    }

    [Fact]
    public void Four_Player_Deal_Of_5_Cards_Starting_With_Third_Player_Will_Be_Correct()
    {
        var deck = new Deck(new List<Card>
        {
            new(Rank.King, Suit.Clubs),
            new(Rank.Ace, Suit.Diamonds),
            new(Rank.Eight, Suit.Hearts),
            new(Rank.Five, Suit.Spades),
            new(Rank.Four, Suit.Diamonds),
            new(Rank.Jack, Suit.Hearts),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Queen, Suit.Spades),
            new(Rank.Seven, Suit.Clubs),
            new(Rank.Six, Suit.Hearts),
            new(Rank.Ten, Suit.Clubs),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Three, Suit.Hearts),
            new(Rank.Three, Suit.Spades),
            new(Rank.Five, Suit.Clubs),
            new(Rank.Three, Suit.Diamonds),
            new(Rank.Five, Suit.Hearts),
            new(Rank.Ace, Suit.Spades),
            new(Rank.Four, Suit.Clubs),
            new(Rank.Two, Suit.Diamonds)
        });

        var handFactory = new Dealer();
        var players = new List<int> { 1, 2, 3, 4 };
        const int handSize = 5;
        var hands = handFactory.Deal(deck, players, players[2], handSize).ToDictionary(p => p.PlayerId, p => p.Hand);

        var player2Actual = hands[players[1]];
        player2Actual[0].Should().Be(new Card(Rank.Five, Suit.Spades));
        player2Actual[1].Should().Be(new Card(Rank.Queen, Suit.Spades));
        player2Actual[2].Should().Be(new Card(Rank.Eight, Suit.Diamonds));
        player2Actual[3].Should().Be(new Card(Rank.Three, Suit.Diamonds));
        player2Actual[4].Should().Be(new Card(Rank.Two, Suit.Diamonds));
        player2Actual.Count.Should().Be(5);

        var player1Actual = hands[players[0]];
        player1Actual[0].Should().Be(new Card(Rank.Eight, Suit.Hearts));
        player1Actual[1].Should().Be(new Card(Rank.Nine, Suit.Diamonds));
        player1Actual[2].Should().Be(new Card(Rank.Ten, Suit.Clubs));
        player1Actual[3].Should().Be(new Card(Rank.Five, Suit.Clubs));
        player1Actual[4].Should().Be(new Card(Rank.Four, Suit.Clubs));
        player1Actual.Count.Should().Be(5);

        var player3Actual = hands[players[2]];
        player3Actual[0].Should().Be(new Card(Rank.King, Suit.Clubs));
        player3Actual[1].Should().Be(new Card(Rank.Four, Suit.Diamonds));
        player3Actual[2].Should().Be(new Card(Rank.Seven, Suit.Clubs));
        player3Actual[3].Should().Be(new Card(Rank.Three, Suit.Hearts));
        player3Actual[4].Should().Be(new Card(Rank.Five, Suit.Hearts));
        player3Actual.Count.Should().Be(5);

        var player4Actual = hands[players[3]];
        player4Actual[0].Should().Be(new Card(Rank.Ace, Suit.Diamonds));
        player4Actual[1].Should().Be(new Card(Rank.Jack, Suit.Hearts));
        player4Actual[2].Should().Be(new Card(Rank.Six, Suit.Hearts));
        player4Actual[3].Should().Be(new Card(Rank.Three, Suit.Spades));
        player4Actual[4].Should().Be(new Card(Rank.Ace, Suit.Spades));
        player4Actual.Count.Should().Be(5);
    }
}