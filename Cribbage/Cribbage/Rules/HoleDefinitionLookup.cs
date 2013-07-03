using System.Collections.Generic;

namespace Cribbage.Rules
{
    public class HoleDefinitionLookup
    {
        public List<HoleDefinitions> Lookup(int hole)
        {
            var holeDefinitions = new List<HoleDefinitions>();

            if (hole == 121) holeDefinitions.Add(HoleDefinitions.GameHole);
            if (hole == 120) holeDefinitions.Add(HoleDefinitions.StinkHole);

            return holeDefinitions;
        }
    }
}
