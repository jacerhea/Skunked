using System;
using System.Collections.Generic;
using Skunked.PlayingCards;

namespace Skunked.Score
{
    public class ScoreCalculatorResult
    {
        public int Score { get; set; }
        public int FifteenScore { get; set; }
        public int PairScore { get; set; }
        public int RunScore { get; set; }
        public int FlushScore { get; set; }
        public int NobScore { get; set; }
        public IList<IList<Card>> Fifteens { get; }
        public IList<IList<Card>> Pairs { get; }
        public IList<IList<Card>> Runs { get; }
        public IList<Card> Flushes { get; }
        public IList<Card> Nobs { get; }

        public ScoreCalculatorResult(IList<IList<Card>> fifteens, IList<IList<Card>> pairs, IList<IList<Card>> runs, IList<Card> flush, IList<Card> nobs,
            int score = 0, int fifteenScore = 0, int pairScore = 0, int runScore = 0, int flushScore = 0, int nobScore = 0)
        {
            if (score < 0) throw new ArgumentOutOfRangeException(nameof(score));
            if (fifteenScore < 0) throw new ArgumentOutOfRangeException(nameof(fifteenScore));
            if (pairScore < 0) throw new ArgumentOutOfRangeException(nameof(pairScore));
            if (runScore < 0) throw new ArgumentOutOfRangeException(nameof(runScore));
            if (flushScore < 0) throw new ArgumentOutOfRangeException(nameof(flushScore));
            if (nobScore < 0) throw new ArgumentOutOfRangeException(nameof(nobScore));

            Score = score;
            FifteenScore = fifteenScore;
            PairScore = pairScore;
            RunScore = runScore;
            FlushScore = flushScore;
            NobScore = nobScore;

            Fifteens = fifteens ?? throw new ArgumentNullException(nameof(fifteens));
            Pairs = pairs ?? throw new ArgumentNullException(nameof(pairs));
            Runs = runs ?? throw new ArgumentNullException(nameof(runs));
            Flushes = flush ?? throw new ArgumentNullException(nameof(flush));
            Nobs = nobs ?? throw new ArgumentNullException(nameof(nobs));
        }
    }
}
