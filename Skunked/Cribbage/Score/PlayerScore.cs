using System;
using Cribbage.Player;


namespace Cribbage.Score
{
    public class PlayerScore
    {
        public delegate void ScoreEventHandler(object sender, GameScoreEventArgs e);
        public event ScoreEventHandler ScoreChangedEvent;

        public ICribPlayer Player { get; private set; }
        public GameScore Score { get; private set; }

        public PlayerScore(ICribPlayer player, GameScore gameScore)
        {
            if (player == null) throw new ArgumentNullException("player");
            if (gameScore == null) throw new ArgumentNullException("gameScore");
            Player = player;
            Score = gameScore;

            Score.ScoreChanged += ScoreChanged;
        }

        private void ScoreChanged(object sender, GameScoreEventArgs args)
        {
            if(ScoreChangedEvent != null)
            {
                ScoreChangedEvent(this, args);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Player.Name, Score.Value);
        }
    }
}