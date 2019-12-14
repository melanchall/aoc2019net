using System;
using Aoc2019Net.Days;

namespace Aoc2019Net
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = new Day10();

            Console.WriteLine($"Solving day {day.DayNumber}...");
            var part1 = day.SolvePart1();
            Console.WriteLine($"Part 1 solution: {part1}");

            var part2 = day.SolvePart2();
            Console.WriteLine($"Part 2 solution: {part2}");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
