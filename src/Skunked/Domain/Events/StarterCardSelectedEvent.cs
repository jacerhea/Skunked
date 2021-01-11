using Skunked.Cards;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class StarterCardSelectedEvent : StreamEvent
    {
        public Card Starter { get; set; }
    }
}
