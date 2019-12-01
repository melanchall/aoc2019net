using System.IO;
using System.Linq;

namespace Aoc2019Net
{
    internal abstract class Day
    {
        protected Day()
        {
            DayNumber = int.Parse(GetType().Name.Substring(3));
        }

        public int DayNumber { get; }

        public (object Part1, object Part2) Solve()
        {
            var part1 = SolvePart1(out var part1Artifacts);
            var part2 = SolvePart2(part1Artifacts);
            return (part1, part2);
        }

        protected abstract object SolvePart1(out object artifacts);

        protected abstract object SolvePart2(object part1Artifacts);

        protected string[] GetInputLines() => File.ReadAllLines(GetInputFilePath());

        protected int[] GetInputNumbers() => GetInputLines().Select(int.Parse).ToArray();

        private string GetInputFilePath() => Path.Combine("Inputs", $"Day{DayNumber}.txt");
    }
}
