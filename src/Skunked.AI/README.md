# Skunked AI strategy

`AiPlayer.OptimizedPlayer` is a simple deterministic baseline. It does not model an opponent or search an entire game.

- **Discard:** `CardToss.maxAverage` considers every four-card keep and every starter outside the original hand. It keeps the combination with the highest average show score and discards the other cards. It does not estimate crib points, crib ownership, or pegging opportunities. Ties use the first combination in the supplied hand order.
- **Pegging:** the player considers cards that keep the current count at or below 31 and chooses the one with the highest immediate pegging score. Ties use the first card in hand order. If no card can be played, it throws `InvalidOperationException`; `GameRunner` should only ask a player for a card when that player has a legal move.
- **Cut and counting:** the player chooses the first offered opening-cut card and counts hands using `ScoreCalculator`. Crib counting uses the crib-specific default on `IGameRunnerPlayer`.

`CardToss.maxAverage` and `minAverage` accept four, five, or six distinct standard cards. Four cards produce an empty discard; five and six produce one and two cards respectively. Invalid hands throw `ArgumentException`. `minAverage` chooses the lowest expected show score and is intended for comparisons.

The unfinished `CardToss.optimisticDecision` function has been removed. It returned a single arbitrary card and did not implement an optimistic strategy. Call `maxAverage` for a complete discard selection instead; this is a breaking API change for callers of the removed function.
