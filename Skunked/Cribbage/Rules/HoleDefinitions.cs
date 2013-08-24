using System;

namespace Cribbage.Rules
{
    [Flags]
    public enum HoleDefinitions
    {
        NoSpecialHole = 1 << 0,
        GameHole = 1 << 1,
        StinkHole = 1 << 2,
        FirstStreet = 1 << 3,
        SecondStreet = 1 << 4,
        ThirdStreet = 1 << 5,
        FourthStreet = 1 << 6,
    }
}
