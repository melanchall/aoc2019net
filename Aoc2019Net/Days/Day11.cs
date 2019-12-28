using Aoc2019Net.Common;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day11 : Day
    {
        private static readonly Dictionary<(int, int), (int, int)> LeftTurns = new Dictionary<(int, int), (int, int)>
        {
            [(0, 1)] = (-1, 0),
            [(0, -1)] = (1, 0),
            [(1, 0)] = (0, 1),
            [(-1, 0)] = (0, -1)
        };

        private static readonly Dictionary<(int, int), (int, int)> RightTurns = new Dictionary<(int, int), (int, int)>
        {
            [(0, 1)] = (1, 0),
            [(0, -1)] = (-1, 0),
            [(1, 0)] = (0, -1),
            [(-1, 0)] = (0, 1)
        };

        public override object SolvePart1()
        {
            var points = GetPaintedPanels(0, out _);
            return points.Take(points.Count - 1).Distinct().Count();
        }

        public override object SolvePart2()
        {
            GetPaintedPanels(1, out var colors);

            var minX = colors.Keys.Min(c => c.X);
            var maxX = colors.Keys.Max(c => c.X);
            var minY = colors.Keys.Min(c => c.Y);
            var maxY = colors.Keys.Max(c => c.Y);

            var width = maxX - minX + 1;
            var height = maxY - minY + 1;

            var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            foreach (var c in colors)
            {
                bitmap.SetPixel(
                    c.Key.X - minX,
                    -c.Key.Y + maxY,
                    c.Value == 0 ? Color.Black : Color.White);
            }

            bitmap.Save("Day11Output.bmp");

            // Read manually from image
            return "ZLEBKJRA";
        }

        private ICollection<(int X, int Y)> GetPaintedPanels(int startColor, out Dictionary<(int X, int Y), int> colors)
        {
            var program = GetInputIntcodeProgram();

            var position = (X: 0, Y: 0);
            var outputMode = 0;
            var points = new List<(int, int)> { position };
            var direction = (X: 0, Y: 1);
            
            var internalColors = new Dictionary<(int, int), int>
            {
                [position] = startColor
            };

            IntcodeComputer.ExecuteProgram(program, new IntcodeComputerParameters
            {
                ExtendProgram = true,
                GetInputValue = () =>
                {
                    if (!internalColors.TryGetValue(position, out var currentColor))
                        internalColors.Add(position, currentColor = 0);

                    return currentColor;
                },
                OnOutput = v =>
                {
                    if (outputMode == 0)
                        internalColors[position] = (int)v;
                    else if (outputMode == 1)
                    {
                        var turns = v == 0 ? LeftTurns : RightTurns;
                        direction = turns[direction];
                        position = (position.X + direction.X, position.Y + direction.Y);
                        points.Add(position);
                    }

                    outputMode++;
                    if (outputMode > 1)
                        outputMode = 0;
                }
            });

            colors = internalColors;
            return points;
        }
    }
}
