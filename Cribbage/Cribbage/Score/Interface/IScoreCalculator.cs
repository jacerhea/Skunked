using System.Collections.Generic;
using Games.Domain.MainModule.Entities.PlayingCards;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.Score
{
    public interface IScoreCalculator
    {
        int CountCut(ICard cut);
        CribScoreCalculatorResult CountShowScore(ICard cutCard, IEnumerable<ICard> playerHand);
        List<IList<ICard>> CountFifteens(Dictionary<int, List<IList<ICard>>> combinationsToCount);
        List<ICard> CountFlush(IEnumerable<ICard> playersHand, ICard cutCard);
        List<IList<ICard>> CountPairs(Dictionary<int, List<IList<ICard>>> combinationsToCheck);
        List<IList<ICard>> CountRuns(Dictionary<int, List<IList<ICard>>> combinationsToCount);
        int CountThePlay(IList<ICard> pile);
        int SumValues(IEnumerable<ICard> cards);
        int GetGoValue();
        bool IsFifteen(IList<ICard> cards);
        bool AreSameKind(IEnumerable<ICard> cards);
        bool IsRun(IList<ICard> combo);
        bool AreContinuous(IEnumerable<int> values);
        Dictionary<int, List<IList<ICard>>> GetCombinations(IList<ICard> sourceSet);
        IList<ICard> Nobs(IEnumerable<ICard> cards, ICard starterCard);
    }
}