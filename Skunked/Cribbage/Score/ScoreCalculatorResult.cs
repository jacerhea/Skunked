using System;
using System.Collections.Generic;

namespace Cribbage.Score
{
    public class ScoreCalculatorResult
    {
        public int Score { get; set; }
        public int FifteenScore { get; set; }
        public int PairScore { get; set; }
        public int RunScore { get; set; }
        public int FlushScore { get; set; }
        public int NobScore { get; set; }
        public IList<IList<Card>> Fifteens { get; private set; }
        public IList<IList<Card>> Pairs { get; private set; }
        public IList<IList<Card>> Runs { get; private set; }
        public IList<Card> Flushes { get; private set; }
        public IList<Card> Nobs { get; private set; }

        public ScoreCalculatorResult(IList<IList<Card>> fifteens, IList<IList<Card>> pairs, IList<IList<Card>> runs, IList<Card> flush, IList<Card> nobs,
            int score = 0, int fifteenScore = 0, int pairScore = 0, int runScore = 0, int flushScore = 0, int nobScore = 0)
        {
            if (fifteens == null) throw new ArgumentNullException("fifteens");
            if (pairs == null) throw new ArgumentNullException("pairs");
            if (runs == null) throw new ArgumentNullException("runs");
            if (flush == null) throw new ArgumentNullException("flush");
            if (nobs == null) throw new ArgumentNullException("nobs");

            if (score < 0) throw new ArgumentOutOfRangeException("score");
            if (fifteenScore < 0) throw new ArgumentOutOfRangeException("fifteenScore");
            if (pairScore < 0) throw new ArgumentOutOfRangeException("pairScore");
            if (runScore < 0) throw new ArgumentOutOfRangeException("runScore");
            if (flushScore < 0) throw new ArgumentOutOfRangeException("flushScore");
            if (nobScore < 0) throw new ArgumentOutOfRangeException("nobScore");

            Score = score;
            FifteenScore = fifteenScore;
            PairScore = pairScore;
            RunScore = runScore;
            FlushScore = flushScore;
            NobScore = nobScore;

            Fifteens = fifteens;
            Pairs = pairs;
            Runs = runs;
            Flushes = flush;
            Nobs = nobs;
        }
    }
}
