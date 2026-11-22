namespace Skunked;

// Snapshots intentionally share only immutable cards and rules.
internal static class GameStateSnapshot
{
    internal static GameState Copy(GameState s) => s with
    {
        PlayerIds = s.PlayerIds.ToList(),
        IndividualScores = s.IndividualScores.Select(x => x with { }).ToList(),
        TeamScores = s.TeamScores.Select(x => x with { Players = x.Players.ToList() }).ToList(),
        OpeningRound = s.OpeningRound with { Deck = s.OpeningRound.Deck.ToList(), EligiblePlayers = s.OpeningRound.EligiblePlayers.ToList(), CutCards = s.OpeningRound.CutCards.ToList() },
        Rounds = s.Rounds.Select(r => r with
        {
            DealtCards = r.DealtCards.Select(CopyHand).ToList(), Hands = r.Hands.Select(CopyHand).ToList(),
            ThePlay = r.ThePlay.Select(p => p.Select(x => x with { }).ToList()).ToList(),
            Crib = r.Crib.ToList(), ShowScores = r.ShowScores.Select(x => x with { }).ToList(),
            PreRound = r.PreRound is null ? null : r.PreRound with { Deck = r.PreRound.Deck.ToList() }
        }).ToList()
    };
    internal static PlayerHand CopyHand(PlayerHand hand) => new(hand.PlayerId, hand.Hand.ToList());
}
