namespace Skunked.Exceptions
{
    public enum InvalidCribbageOperation
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
        InvalidShowCount,
        PlayerHasAlreadyCounted,
        GameFinished,
        InvalidRequest
    }
}
