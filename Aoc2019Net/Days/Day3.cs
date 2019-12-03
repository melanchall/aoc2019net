using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2019Net.Days
{
    public sealed class Day3 : Day
    {
        private static readonly Dictionary<string, (int X, int Y)> TurnOffsets = new Dictionary<string, (int, int)>
        {
            ["U"] = (0, 1),
            ["D"] = (0, -1),
            ["L"] = (-1, 0),
            ["R"] = (1, 0)
        };

        private static readonly Regex TurnRegex = new Regex($@"({string.Join("|", TurnOffsets.Keys)})(\d+)");

        protected override object SolvePart1() => GetIntersections(out _, out _)
            .Min(c => Math.Abs(c.X) + Math.Abs(c.Y));

        protected override object SolvePart2() => GetIntersections(out var points1, out var points2)
            .Min(c => points1.FirstOrDefault(p => p.X == c.X && p.Y == c.Y).Steps +
                      points2.FirstOrDefault(p => p.X == c.X && p.Y == c.Y).Steps);

        private IEnumerable<(int X, int Y)> GetIntersections(
            out ICollection<(int X, int Y, int Steps)> points1,
            out ICollection<(int X, int Y, int Steps)> points2)
        {
            var lines = GetInputLines();

            var turns1 = ParseTurns(lines[0]);
            points1 = GetWirePoints(turns1);

            var turns2 = ParseTurns(lines[1]);
            points2 = GetWirePoints(turns2);
            
            return points1.Select(p => (p.X, p.Y))
                .Intersect(points2.Select(p => (p.X, p.Y)))
                .Where(c => c.X != 0 || c.Y != 0)
                .ToArray();
        }

        private static IEnumerable<(string Turn, int Steps)> ParseTurns(string turnsLine)
        {
            var tokens = turnsLine.Split(",");
            return from t in tokens
                   let m = TurnRegex.Match(t.Trim())
                   let g = m.Groups
                   select (g[1].Value, int.Parse(g[2].Value));
        }

        private static ICollection<(int X, int Y, int Steps)> GetWirePoints(IEnumerable<(string Turn, int Steps)> turns)
        {
            var result = new HashSet<(int X, int Y, int Steps)>();

            var currentPoint = (X: 0, Y: 0);
            var steps = 0;

            foreach (var turn in turns)
            {
                var offset = TurnOffsets[turn.Turn];
                var currentPoints = Enumerable
                    .Range(1, turn.Steps)
                    .Select(i => (X: currentPoint.X + i * offset.X, Y: currentPoint.Y + i * offset.Y, Steps: ++steps))
                    .ToArray();
                
                foreach (var point in currentPoints)
                {
                    result.Add(point);
                }

                var lastPoint = currentPoints.Last();
                currentPoint = (lastPoint.X, lastPoint.Y);
            }

            return result;
        }
    }
}
