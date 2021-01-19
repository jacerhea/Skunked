using System.Collections.Generic;
using Skunked.Cards;
using Skunked.Players;

namespace Skunked.Domain.State
{

    public class PreRound
    {
        public List<Card> Deck { get; set; } = new ();
    }
}
