using Skunked.PlayingCards;
using Skunked.State;

namespace Skunked.Commands
{
    public class ShuffleDeckCommandArgs : CommandArgsBase
    {
        public GameState GameState { get; private set; }
        public int Round { get; private set; }
        public Deck Deck { get; private set; }

        public ShuffleDeckCommandArgs(GameState gameState, int round, Deck deck) : base(gameState, round, round)
        {
            GameState = gameState;
            Round = round;
            Deck = deck;
        }
    }
}
