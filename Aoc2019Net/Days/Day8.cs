using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day8 : Day
    {
        private const int LayerWidth = 25;
        private const int LayerHeight = 6;

        public override object SolvePart1()
        {
            var layers = GetLayers();
            var targetLayer = layers.OrderBy(l => CountDigit(l, 0)).First();
            return CountDigit(targetLayer, 1) * CountDigit(targetLayer, 2);
        }

        public override object SolvePart2()
        {
            var layers = GetLayers();
            var image = new Bitmap(LayerWidth, LayerHeight, PixelFormat.Format24bppRgb);

            for (int y = 0; y < LayerHeight; y++)
            {
                for (int x = 0; x < LayerWidth; x++)
                {
                    var colorNumber = layers.Select(l => l[y * LayerWidth + x]).First(d => d != 2);
                    image.SetPixel(x, y, colorNumber == 0 ? Color.Black : Color.White);
                }
            }

            image.Save("Day8Output.bmp");

            // Read manually from image
            return "YGRYZ";
        }

        private int[][] GetLayers()
        {
            var digits = GetInput().Select(c => int.Parse(c.ToString())).ToArray();

            var layerDigitsCount = LayerWidth * LayerHeight;
            var layersCount = digits.Length / layerDigitsCount;
            return digits.Select((d, i) => new { GroupNumber = i / layerDigitsCount, Digit = d })
                         .GroupBy(d => d.GroupNumber)
                         .Select(g => g.Select(d => d.Digit).ToArray())
                         .ToArray();
        }

        private static int CountDigit(int[] layer, int digit) => layer.Count(d => d == digit);
    }
}
