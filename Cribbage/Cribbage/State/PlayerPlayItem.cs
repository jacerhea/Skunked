using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Games.Domain.MainModule.Entities.CardGames.Player;
using Games.Domain.MainModule.Entities.PlayingCards;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.State
{
#if !SILVERLIGHT
    [DataContract]
#endif
    public class PlayerPlayItem
    {
#if !SILVERLIGHT
        [DataMember]
#endif
        public int Player { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public SerializableCard Card { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int Score { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int? NextPlayer { get; set; }
    }
}
