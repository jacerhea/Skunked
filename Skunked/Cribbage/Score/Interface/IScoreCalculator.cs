using System.Collections.Generic;

namespace Skunked.Score.Interface
{
    public interface IScoreCalculator
    {
        int CountCut(Card cut);
        ScoreCalculatorResult CountShowScore(Card cutCard, IEnumerable<Card> playerHand);
        List<IList<Card>> CountFifteens(Dictionary<int, List<IList<Card>>> combinationsToCount);
        List<Card> CountFlush(List<Card> playersHand, Card cutCard);
        List<IList<Card>> CountPairs(Dictionary<int, List<IList<Card>>> combinationsToCheck);
        List<IList<Card>> CountRuns(Dictionary<int, List<IList<Card>>> combinationsToCount);
        int CountThePlay(IList<Card> pile);
        int SumValues(IEnumerable<Card> cards);
        int GetGoValue();
        bool IsFifteen(IList<Card> cards);
        bool AreSameKind(IEnumerable<Card> cards);
        bool IsRun(IList<Card> combo);
        bool AreContinuous(IEnumerable<int> values);
        Dictionary<int, List<IList<Card>>> GetCombinations(IList<Card> sourceSet);
        IList<Card> Nobs(IEnumerable<Card> cards, Card starterCard);
    }
}