namespace Skunked.Domain.Events
{
    public class HandCountedEvent : StreamEvent
    {
        /// <summary>
        /// The player id. 
        /// </summary>
        public int PlayerId { get; set; }
        public int CountedScore { get; set; }
    }
}