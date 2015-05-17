using Skunked.State;
using Skunked.State.Events;

namespace Skunked.Commands
{
    public class ShuffleDeckCommandArgs : CommandArgsBase
    {
        public ShuffleDeckCommandArgs(GameEventStream eventStream, GameState gameState, int playerId, int round)
            : base(eventStream, gameState, playerId, round)
        {
        }
    }
}
