using System.Collections.Generic;
using System.Linq;
using Aoc2019Net.Common;

namespace Aoc2019Net.Days
{
    public sealed class Day17 : Day
    {
        private static class CameraOutputTypes
        {
            public const long Scaffold = 35;  // #
            public const long OpenSpace = 46; // .
            public const long NewLine = 10;   // new line
        }

        public override object SolvePart1()
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

            var intersections = from cell in map
                                where cell.Value == CameraOutputTypes.Scaffold
                                where cell.Key.GetNearestLocations().All(l => map.TryGetValue(l, out var cameraOutput) && cameraOutput == CameraOutputTypes.Scaffold)
                                select cell.Key;

            return intersections.Sum(i => i.X * i.Y);
        }

        public override object SolvePart2()
        {
            return null;
        }
    }
}
