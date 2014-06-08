using System.Collections.Generic;
using Skunked.Players;

namespace Skunked.AI
{
    public interface IAIPlayerFactory
    {
        List<Player> CreatePlayers(int numberOfPlayers);
        Player CreatePlayer(AIDifficulty difficulty, string name);
    }
}