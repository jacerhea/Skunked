using System.Collections.Generic;
using Cribbage.Players;

namespace Cribbage.AI
{
    public interface IAIPlayerFactory
    {
        List<Player> CreatePlayers(int numberOfPlayers);
        Player CreatePlayer(AIDifficulty difficulty, string name);
    }
}