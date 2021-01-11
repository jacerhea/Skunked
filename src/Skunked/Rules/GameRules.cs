using System;

namespace Skunked.Rules
{
    /// <summary>
    /// Set of Cribbage rules.
    /// </summary>
    public class GameRules
    {
        private readonly WinningScoreType _winningScore;


        /// <summary>
        /// Number of players
        /// </summary>
        public int NumberOfPlayers { get; }

        /// <summary>
        /// The score a player needs to reach first to win the game.
        /// </summary>
        public int WinningScore => _winningScore == WinningScoreType.Standard121 ? 121 : 61;

        /// <summary>
        /// Size of initial hand that dealer gives to each player
        /// </summary>
        public int DealSize => NumberOfPlayers == 2 ? 6 : 5;


        /// <summary>
        /// Size of hand after throwing cards to crib.
        /// </summary>
        public static int HandSize => 4;


        public GameRules() : this(WinningScoreType.Standard121, 2)
        {
        }

        public GameRules(WinningScoreType winningScore, int numberOfPlayers)
        {
            if (numberOfPlayers != 2 && numberOfPlayers != 4) { throw new ArgumentOutOfRangeException(nameof(numberOfPlayers)); }
            NumberOfPlayers = numberOfPlayers;
            _winningScore = winningScore;
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Points
        {
            /// <summary>
            /// Points for laying the last card in a play where the count is under 31.
            /// </summary>
            public static int Go => 1;

            /// <summary>
            /// Max count a play can reach before starting a new play.
            /// </summary>
            public static int MaxPlayCount => 31;

            /// <summary>
            /// Combination of two or more cards totaling exactly fifteen
            /// </summary>
            public static int Fifteen => 2;

            /// <summary>
            /// Jack is cut as the starter card. Points are awarded to dealer.
            /// </summary>
            public static int Nibs => 2;

            /// <summary>
            /// One point for holding the Jack of the same suit as the starter card
            /// </summary>
            public static int Nobs => 1;

            /// <summary>
            /// Two cards of a kind
            /// </summary>
            public static int Pair => 2;

            /// <summary>
            /// Three cards of a kind
            /// </summary>
            public static int PairRoyal => 6;

            /// <summary>
            /// Four cards of a kind
            /// </summary>
            public static int DoublePairRoyal => 12;

            /// <summary>
            /// All four cards in the hand are of the same suit
            /// </summary>
            public static int Flush => 4;
        }
    }
}