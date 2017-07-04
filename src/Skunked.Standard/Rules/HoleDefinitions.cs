using System;

namespace Skunked.Rules
{
    [Flags]
    public enum HoleDefinitions
    {
        Starter = 1,
        NoSpecialHole = 2,
        GameHole = 3,
        StinkHole = 4,
        FirstStreet = 5,
        SecondStreet = 6,
        ThirdStreet = 7,
        FourthStreet = 8,
    }
}
