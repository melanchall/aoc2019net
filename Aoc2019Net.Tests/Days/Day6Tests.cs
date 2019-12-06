using Aoc2019Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2019Net.Tests.Days
{
    [TestFixture]
    [DayDataPart1(147807)]
    [DayDataPart1(@"
        COM)B
        B)C
        C)D
        D)E
        E)F
        B)G
        G)H
        D)I
        E)J
        J)K
        K)L", 42)]
    [DayDataPart2(229)]
    [DayDataPart2(@"
        COM)B
        B)C
        C)D
        D)E
        E)F
        B)G
        G)H
        D)I
        E)J
        J)K
        K)L
        K)YOU
        I)SAN", 4)]
    public sealed class Day6Tests : DayTests<Day6Tests>
    {
    }
}
