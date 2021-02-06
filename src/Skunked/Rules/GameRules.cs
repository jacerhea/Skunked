using System;

namespace Skunked.Rules
{
    /// <summary>
    /// Set of Cribbage rules.
    /// </summary>
    public class GameRules
    {
        private readonly WinningScore _winningScore;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameRules"/> class.
        /// </summary>
        public GameRules()
            : this(Rules.WinningScore.Standard121)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameRules"/> class.
        /// </summary>
        /// <param name="winningScore">The winning score.</param>
        public GameRules(WinningScore winningScore)
        {
            _winningScore = winningScore;
        }

        /// <summary>
        /// Gets size of hand after throwing cards to crib.
        /// </summary>
        public static int HandSize => 4;

        /// <summary>
        /// Gets the score a player needs to reach first to win the game.
        /// </summary>
        public int WinningScore => _winningScore == Rules.WinningScore.Standard121 ? 121 : 61;


        /// <summary>
        /// Gets size of initial hand that dealer gives to each player.
        /// </summary>
        /// <param name="numberOfPlayers">Number of players.</param>
        /// <returns>Number of cards to be dealt to each player.</returns>
        public int GetDealSize(int numberOfPlayers) => numberOfPlayers == 2 ? 6 : 5;

        /// <summary>
        /// The points scored for all combinations.
        /// </summary>
        public static class Points
        {
            /// <summary>
            /// Gets points for laying the last card in a play where the count is under 31.
            /// </summary>
            public static int Go => 1;

            /// <summary>
            /// Gets max count a play can reach before starting a new play.
            /// </summary>
            public static int MaxPlayCount => 31;

            /// <summary>
            /// Gets combination of two or more cards totaling exactly fifteen.
            /// </summary>
            public static int Fifteen => 2;

            /// <summary>
            /// Gets jack is cut as the starter card. Points are awarded to dealer.
            /// </summary>
            public static int Nibs => 2;

            /// <summary>
            /// Gets one point for holding the Jack of the same suit as the starter card.
            /// </summary>
            public static int Nobs => 1;

            /// <summary>
            /// Gets two cards of a kind.
            /// </summary>
            public static int Pair => 2;

            /// <summary>
            /// Gets three cards of a kind.
            /// </summary>
            public static int PairRoyal => 6;

            /// <summary>
            /// Gets four cards of a kind.
            /// </summary>
            public static int DoublePairRoyal => 12;

            /// <summary>
            /// Gets all four cards in the hand are of the same suit.
            /// </summary>
            public static int Flush => 4;
        }
    }
}