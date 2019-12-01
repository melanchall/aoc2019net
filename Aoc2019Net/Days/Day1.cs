using System.Linq;

namespace Aoc2019Net.Days
{
    internal sealed class Day1 : Day
    {
        public Day1()
            : base(1)
        {
        }

        protected override object SolvePart1(out object artifacts)
        {
            artifacts = null;
            return GetInputNumbers().Select(GetFuel).Sum();
        }

        protected override object SolvePart2(object part1Artifacts)
        {
            return GetInputNumbers().Select(GetFuelRequirement).Sum();
        }

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
