using System.Collections.Generic;
using Skunked.Players;

namespace Skunked.AI
{
    public interface IAiPlayerFactory
    {
        List<Player> CreatePlayers(int numberOfPlayers);
        Player CreatePlayer(AiDifficulty difficulty, string name);
    }
}