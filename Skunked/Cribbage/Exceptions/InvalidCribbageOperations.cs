namespace Skunked.Exceptions
{
    public enum InvalidCribbageOperations
    {
        CutCardPlayerAlreadyCut,
        CutCardCardAlreadyCut,
        NotPlayersTurn,
        CardHasBeenPlayed,
        CardsHaveBeenThrown,
        InvalidPlayer,
        InvalidStateForPlay,
        InvalidCard,
        InvalidStateForCount,
        InvalidStateForCribCount,
        PlayerHasAlreadyCounted,
        GameFinished,
        InvalidRequest
    }
}
