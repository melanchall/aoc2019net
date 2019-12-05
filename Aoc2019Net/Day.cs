using System;
using System.IO;
using System.Linq;

namespace Aoc2019Net
{
    public abstract class Day
    {
        protected Day()
        {
            DayNumber = int.Parse(GetType().Name.Substring(3));
        }

        public string Input { get; set; }

        public int DayNumber { get; }

        public (object Part1, object Part2) Solve()
        {
            object part1 = null, part2 = null;

            try { part1 = SolvePart1(); } catch { }
            try { part2 = SolvePart2(); } catch { }

            return (part1, part2);
        }

        protected abstract object SolvePart1();

        protected abstract object SolvePart2();

        protected string[] GetInputLines() => GetInputTokens(Environment.NewLine);

        protected string[] GetInputTokens(string delimiter) => GetInput()
            .Split(delimiter)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Trim())
            .ToArray();

        protected int[] GetInputNumbers(string delimiter) => GetInputTokens(delimiter).Select(t => int.Parse(t)).ToArray();

        private string GetInput() => Input ?? File.ReadAllText(GetInputFilePath());

        private string GetInputFilePath() => Path.Combine("Inputs", $"Day{DayNumber}.txt");
    }
}
