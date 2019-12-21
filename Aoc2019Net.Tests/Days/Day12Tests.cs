using Aoc2019Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2019Net.Tests.Days
{
    [TestFixture]
    [DayDataPart1(11384)]
    [DayDataPart1(@"
        <x=-1, y=0, z=2>
        <x=2, y=-10, z=-7>
        <x=4, y=-8, z=8>
        <x=3, y=5, z=-1>", 179, Parameters = new object[] { 10 })]
    [DayDataPart1(@"
        <x=-8, y=-10, z=0>
        <x=5, y=5, z=10>
        <x=2, y=-7, z=3>
        <x=9, y=-8, z=-3>", 1940, Parameters = new object[] { 100 })]
    [DayDataPart2(null)]
    public sealed class Day12Tests : DayTests<Day12Tests>
    {
    }
}
