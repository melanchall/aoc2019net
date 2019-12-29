using System.Collections.Generic;
using System.Linq;
using Aoc2019Net.Common;

namespace Aoc2019Net.Days
{
    public sealed class Day17 : Day
    {
        private static class CameraOutputTypes
        {
            public const long Scaffold  = 35; // #
            public const long OpenSpace = 46; // .
            public const long NewLine   = 10; // new line
        }

        private static readonly Dictionary<char, (int X, int Y)[]> Offsets = new Dictionary<char, (int X, int Y)[]>
        {
            ['<'] = new[] { (0, 1), (0, -1) },
            ['>'] = new[] { (0, -1), (0, 1) },
            ['^'] = new[] { (-1, 0), (1, 0) },
            ['v'] = new[] { (1, 0), (-1, 0) }
        };

        private static readonly Dictionary<char, char[]> Turns = new Dictionary<char, char[]>
        {
            ['<'] = new[] { 'v', '^' },
            ['>'] = new[] { '^', 'v' },
            ['^'] = new[] { '<', '>' },
            ['v'] = new[] { '>', '<' }
        };

        public override object SolvePart1()
        {
            var map = GetMap();

            var intersections = from cell in map
                                where cell.Value == CameraOutputTypes.Scaffold
                                where cell.Key.GetNearestLocations().All(l => map.TryGetValue(l, out var cameraOutput) && cameraOutput == CameraOutputTypes.Scaffold)
                                select cell.Key;

            return intersections.Sum(i => i.X * i.Y);
        }

        public override object SolvePart2()
        {
            var map = GetMap();

            var robotLocation = map.FirstOrDefault(c => Turns.ContainsKey((char)c.Value)).Key;
            var robotDirection = (char)map[robotLocation];
            map[robotLocation] = CameraOutputTypes.Scaffold;

            var moves = new List<string>();

            while (true)
            {
                var offsets = Offsets[robotDirection];
                var nearestCameraOutputs = offsets
                    .Select(o => map.TryGetValue((robotLocation.X + o.X, robotLocation.Y + o.Y), out var cameraOutput) ? cameraOutput : '\0')
                    .ToArray();

                if (nearestCameraOutputs.All(o => o != CameraOutputTypes.Scaffold))
                    break;

                var directionIndex = nearestCameraOutputs[0] == CameraOutputTypes.Scaffold ? 0 : 1;
                robotDirection = Turns[robotDirection][directionIndex];

                var steps = Enumerable.Range(1, int.MaxValue).First(s =>
                {
                    var point = (robotLocation.X + offsets[directionIndex].X * s, robotLocation.Y + offsets[directionIndex].Y * s);
                    var locations = point.GetNearestLocations().Where(l => map.TryGetValue(l, out var c) && c == CameraOutputTypes.Scaffold).ToArray();
                    return (locations.Length == 2 && locations[0].X != locations[1].X && locations[0].Y != locations[1].Y) || locations.Length == 1;
                });

                robotLocation = (robotLocation.X + offsets[directionIndex].X * steps, robotLocation.Y + offsets[directionIndex].Y * steps);

                moves.Add(directionIndex == 0 ? "L" : "R");
                moves.Add(steps.ToString());
            }

            GetFunctions(moves, out var a, out var b, out var c);

            var mainRoutine = string.Join(",", string.Join(string.Empty, moves)
                .Replace(string.Join(string.Empty, a), "A")
                .Replace(string.Join(string.Empty, b), "B")
                .Replace(string.Join(string.Empty, c), "C")
                .ToArray()) + '\n';

            var input = new[]
            {
                mainRoutine,
                string.Join(",", a) + '\n',
                string.Join(",", b) + '\n',
                string.Join(",", c) + '\n',
                "n\n"
            };

            var index = 0;

            var program = GetInputIntcodeProgram();
            program[0] = 2;

            var mode = 0;

            var result = IntcodeComputer.ExecuteProgram(program, new IntcodeComputerParameters
            {
                ExtendProgram = true,
                GetInputValue = () =>
                {
                    var c = input[mode][index++];
                    if (c == '\n')
                    {
                        mode++;
                        index = 0;
                    }

                    return c;
                }
            });

            return result.Outputs.Last();
        }

        private void GetFunctions(ICollection<string> moves, out ICollection<string> a, out ICollection<string> b, out ICollection<string> c)
        {
            a = b = c = null;

            var path = string.Join(string.Empty, moves);

            bool TryBuildSubPath(int position, int length, out ICollection<string> subPath)
            {
                subPath = moves.Skip(position).Take(length).ToArray();
                return string.Join(",", subPath).Length <= 20;
            };

            var lengths = Enumerable.Range(1, int.MaxValue).Select(i => 2 * i).TakeWhile(l => l < moves.Count).ToArray();
            var positions = Enumerable.Range(0, int.MaxValue).Select(i => 2 * i).TakeWhile(l => l < moves.Count).ToArray();

            foreach (var aPosition in positions)
            {
                foreach (var aLength in lengths)
                {
                    if (!TryBuildSubPath(aPosition, aLength, out a))
                        continue;

                    foreach (var bPosition in positions.Where(p => p > aPosition))
                    {
                        foreach (var bLength in lengths)
                        {
                            if (!TryBuildSubPath(bPosition, bLength, out b))
                                continue;

                            foreach (var cPosition in positions.Where(p => p > bPosition))
                            {
                                foreach (var cLength in lengths)
                                {
                                    if (!TryBuildSubPath(cPosition, cLength, out c))
                                        continue;

                                    var reducedPath = new[]
                                        {
                                            string.Join(string.Empty, a),
                                            string.Join(string.Empty, b),
                                            string.Join(string.Empty, c)
                                        }
                                        .OrderByDescending(s => s.Length)
                                        .Aggregate(path, (result, subPath) => result.Replace(subPath, string.Empty));

                                    if (string.IsNullOrWhiteSpace(reducedPath))
                                        return;
                                }
                            }
                        }
                    }
                }
            }
        }

        private Dictionary<(int X, int Y), long> GetMap()
        {
            var program = GetInputIntcodeProgram();

            var map = new Dictionary<(int X, int Y), long>();
            var x = 0;
            var y = 0;

            var result = IntcodeComputer.ExecuteProgram(program, new IntcodeComputerParameters
            {
                ExtendProgram = true,
                OnOutput = value =>
                {
                    switch (value)
                    {
                        case CameraOutputTypes.NewLine:
                            y++;
                            x = 0;
                            break;
                        default:
                            map[(x, y)] = value;
                            x++;
                            break;
                    }
                }
            });

            return map;
        }
    }
}
