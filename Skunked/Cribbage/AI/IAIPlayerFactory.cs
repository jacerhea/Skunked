using System.Collections.Generic;
using Cribbage.Player;

namespace Cribbage.AI
{
    public interface IAIPlayerFactory
    {
        List<ICribPlayer> CreatePlayers(int numberOfPlayers);
        ICribPlayer CreatePlayer(AIDifficulty difficulty, string name);
    }
}