using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day1 : Day
    {
        protected override object SolvePart1() => GetInputNumbers().Select(GetFuel).Sum();

        protected override object SolvePart2() => GetInputNumbers().Select(GetFuelRequirement).Sum();

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
