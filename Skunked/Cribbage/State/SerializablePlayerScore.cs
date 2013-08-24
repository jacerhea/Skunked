using System.Diagnostics;
using System.Runtime.Serialization;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.State
{
#if !SILVERLIGHT
    [DataContract]
#endif
    [DebuggerDisplay("Player: {Player}, Score: {Score}")]
    public class SerializablePlayerScore
    {
        private int _score;
        private int _winningScore;
#if !SILVERLIGHT
        [DataMember]
#endif
        public int Player { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
            }
        }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int WinningScore
        {
            get
            {
                return _winningScore;
            }
            set
            {
                _winningScore = value;
            }
        }
    }
}
