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
            return (SolvePart1(), SolvePart2());
        }

        protected abstract object SolvePart1();

        protected abstract object SolvePart2();

        protected string[] GetInputTokens(string delimiter) => File.ReadAllText(GetInputFilePath()).Split(delimiter);

        protected string[] GetInputLines() => File.ReadAllLines(GetInputFilePath());

        protected int[] GetInputNumbers() => GetInputLines().Select(int.Parse).ToArray();

        private string GetInputFilePath() => Path.Combine("Inputs", $"Day{DayNumber}.txt");
    }
}
