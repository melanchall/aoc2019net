using System;
using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day1 : Day
    {
        private static readonly string InputTokensDelimiter = Environment.NewLine;

        public override object SolvePart1() => GetInputNumbers(InputTokensDelimiter).Select(GetFuel).Sum();

        public override object SolvePart2() => GetInputNumbers(InputTokensDelimiter).Select(GetFuelRequirement).Sum();

        private static int GetFuel(int mass) => mass / 3 - 2;

        private static int GetFuelRequirement(int mass)
        {
            var total = 0;

            while (GetFuel(mass) >= 0)
            {
                total += mass = GetFuel(mass);
            }

            return total;
        }
    }
}
