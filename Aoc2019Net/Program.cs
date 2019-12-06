using System;
using Aoc2019Net.Days;

namespace Aoc2019Net
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = new Day6();

            Console.WriteLine($"Solving day {day.DayNumber}...");
            var solution = day.Solve();

            Console.WriteLine($"Part 1 solution: {solution.Part1}");
            Console.WriteLine($"Part 2 solution: {solution.Part2}");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
