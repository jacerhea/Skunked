using System.Collections.Generic;
using Skunked.Cards;

namespace Skunked.Domain.State
{

    public class PreRound
    {
        public List<Card> Deck { get; set; } = new ();
    }
}
