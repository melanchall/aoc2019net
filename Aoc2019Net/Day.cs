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
            return (SolvePart1(), SolvePart2());
        }

        protected abstract object SolvePart1();

        protected abstract object SolvePart2();

        protected string[] GetInputTokens(string delimiter) => (Input ?? File.ReadAllText(GetInputFilePath())).Split(delimiter);

        protected string[] GetInputLines() => (Input != null ? Input.Split(Environment.NewLine) : File.ReadAllLines(GetInputFilePath()))
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Trim())
            .ToArray();

        protected int[] GetInputNumbers(string delimiter) => GetInputTokens(delimiter).Select(t => int.Parse(t.Trim())).ToArray();

        private string GetInputFilePath() => Path.Combine("Inputs", $"Day{DayNumber}.txt");
    }
}
