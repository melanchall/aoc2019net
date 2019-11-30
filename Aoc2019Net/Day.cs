using System.IO;

namespace Aoc2019Net
{
    internal abstract class Day
    {
        public Day(int dayNumber)
        {
            DayNumber = dayNumber;
            Input = File.ReadAllText(Path.Combine("Inputs", $"Day{DayNumber}.txt"));
        }

        public int DayNumber { get; }

        public string Input { get; }

        public (object Part1, object Part2) Solve()
        {
            Initialize();
            return (SolvePart1(), SolvePart2());
        }

        protected virtual void Initialize()
        {
        }

        protected abstract object SolvePart1();

        protected abstract object SolvePart2();
    }
}
