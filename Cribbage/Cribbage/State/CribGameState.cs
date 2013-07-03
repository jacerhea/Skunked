using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.State
{
#if !SILVERLIGHT
    [DataContract]
#endif
    public class CribGameState
    {
#if !SILVERLIGHT
        [DataMember]
#endif
        public List<SerializablePlayer> Players { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public List<SerializablePlayerScore> PlayerScores { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public CribGameRules GameRules { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public CribOpeningRoundState OpeningRoundState { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public List<CribRoundState> Rounds { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public DateTime StartedAt { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public DateTime LastUpdated { get; set; }

        public CribRoundState CurrentRound()
        {
            return this.Rounds.MaxBy(round => round.Round);
        }

        public bool IsGameFinished()
        {
            return PlayerScores.Any(ps => ps.Score >= GameRules.WinningScore);
        }
    }
}
