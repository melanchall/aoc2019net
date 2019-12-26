using System;
using System.Collections.Generic;
using System.Linq;
using Aoc2019Net.Common;

namespace Aoc2019Net.Days
{
    public sealed class Day15 : Day
    {
        private static class CellType
        {
            public const int Wall = 0;
            public const int Empty = 1;
            public const int OxygenStation = 2;
        }

        private const string InputTokensDelimiter = ",";

        public override object SolvePart1()
        {
            var map = BuildMap(out var oxygenStationLocation);
            var steps = TraceRoutes((0, 0), map);
            return steps[oxygenStationLocation];
        }

        public override object SolvePart2()
        {
            var map = BuildMap(out var oxygenStationLocation);
            var steps = TraceRoutes(oxygenStationLocation, map);
            return steps.Values.Max();
        }

        private Dictionary<(int, int), int> TraceRoutes((int X, int Y) fromPoint, Dictionary<(int X, int Y), long> map)
        {
            var steps = new Dictionary<(int, int), int>();
            TraceRoutes(fromPoint, map, steps, 0);
            return steps;
        }

        private void TraceRoutes((int X, int Y) currentPoint, Dictionary<(int X, int Y), long> map, Dictionary<(int, int), int> steps, int currentSteps)
        {
            if (steps.ContainsKey(currentPoint) || map[currentPoint] == CellType.Wall)
                return;

            steps[currentPoint] = currentSteps++;

            foreach (var nextPoint in GetNearestLocations(currentPoint))
            {
                TraceRoutes(nextPoint, map, steps, currentSteps);
            }
        }

        private Dictionary<(int X, int Y), long> BuildMap(out (int X, int Y) oxygenStationLocation)
        {
            var program = GetInputNumbers(InputTokensDelimiter);

            var droidLocation = (X: 0, Y: 0);
            var nextDroidLocation = (X: 0, Y: 0);

            var map = new Dictionary<(int X, int Y), long> { [droidLocation] = 1 };
            var visitedCount = new Dictionary<(int, int), int> { [droidLocation] = 1 };

            var programResult = IntcodeComputer.ExecuteProgram(program, new IntcodeComputerParameters
            {
                ExtendProgram = true,
                GetInputValue = () =>
                {
                    if (IsMapBuilt(map))
                        return 0;

                    var possibleNextDroidLocations = GetNearestLocations(droidLocation)
                        .Select((l, i) => new { Location = l, Direction = i + 1 })
                        .Where(l => !map.ContainsKey(l.Location) || map[l.Location] != CellType.Wall)
                        .ToDictionary(l => l.Direction, l => l.Location);

                    var direction = possibleNextDroidLocations.FirstOrDefault(l => !visitedCount.ContainsKey(l.Value)).Key;
                    if (direction == default(KeyValuePair<int, (int, int)>).Key)
                        direction = possibleNextDroidLocations
                            .OrderBy(l => visitedCount[l.Value])
                            .ThenBy(l => Convert.ToInt32(l.Value == droidLocation))
                            .FirstOrDefault()
                            .Key;

                    nextDroidLocation = possibleNextDroidLocations[direction];
                    return direction;
                },
                OnOutput = statusCode =>
                {
                    map[nextDroidLocation] = statusCode;
                    if (statusCode != CellType.Wall)
                    {
                        droidLocation = nextDroidLocation;
                        if (!visitedCount.TryAdd(droidLocation, 0))
                            visitedCount[droidLocation]++;
                    }
                }
            });

            oxygenStationLocation = map.FirstOrDefault(c => c.Value == CellType.OxygenStation).Key;
            return map;
        }

        private static bool IsMapBuilt(Dictionary<(int X, int Y), long> map) => map
            .Where(cell => cell.Value != CellType.Wall)
            .SelectMany(cell => GetNearestLocations(cell.Key))
            .All(point => map.ContainsKey(point));

        private static IEnumerable<(int X, int Y)> GetNearestLocations((int X, int Y) point) => new[]
        {
            (point.X, point.Y + 1),
            (point.X, point.Y - 1),
            (point.X - 1, point.Y),
            (point.X + 1, point.Y)
        };
    }
}
