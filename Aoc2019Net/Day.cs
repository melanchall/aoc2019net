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

        public abstract object SolvePart1();

        public abstract object SolvePart2();

        protected string[] GetInputLines() => GetInputTokens(Environment.NewLine);

        protected string[] GetInputTokens(string delimiter) => GetInput()
            .Split(delimiter)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Trim())
            .ToArray();

        protected int[] GetInputNumbers(string delimiter) => GetInputTokens(delimiter).Select(t => int.Parse(t)).ToArray();

        protected string GetInput() => Input ?? File.ReadAllText(GetInputFilePath());

        private string GetInputFilePath() => Path.Combine("Inputs", $"Day{DayNumber}.txt");
    }
}
