using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day10 : Day
    {
        private sealed class VectorsComparer : IEqualityComparer<(double X, double Y)>
        {
            private const double Epsilon = 0.00001;

            public bool Equals((double X, double Y) vector1, (double X, double Y) vector2)
            {
                return Math.Abs(vector1.X - vector2.X) < Epsilon && Math.Abs(vector1.Y - vector2.Y) < Epsilon;
            }

            public int GetHashCode((double X, double Y) vector)
            {
                return 0;
            }
        }

        public override object SolvePart1()
        {
            GetMonitoringStationLocation(out var directlyDetectableAsteroidsLocations, out _);
            return directlyDetectableAsteroidsLocations;
        }

        public override object SolvePart2()
        {
            var monitoringStationLocation = GetMonitoringStationLocation(out _, out var orderedAsteroidsGroups);
            var asteroidsByAngle = orderedAsteroidsGroups
                .Where(asteroids => asteroids.Any())
                .OrderBy(asteroids => GetAngle(Subtract(asteroids.First(), monitoringStationLocation)))
                .Select(asteroids => new Queue<(int X, int Y)>(asteroids))
                .ToArray();

            var targetLocation = (X: 0, Y: 0);

            for (int i = 0, j = 0; j < 200; i++)
            {
                if (i >= asteroidsByAngle.Length)
                    i = 0;

                var asteroids = asteroidsByAngle[i];
                if (!asteroids.Any())
                    continue;

                targetLocation = asteroids.Dequeue();
                j++;
            }

            return targetLocation.X * 100 + targetLocation.Y;
        }

        private (int X, int Y) GetMonitoringStationLocation(out int directlyDetectableAsteroidsLocations, out ICollection<(int X, int Y)[]> orderedAsteroidsGroups)
        {
            var lines = GetInputLines();

            var asteroidsLocations = lines
                .SelectMany((l, y) => l.Select((c, x) => new { Symbol = c, X = x, Y = y })
                                       .Where(s => s.Symbol == '#')
                                       .Select(s => (s.X, s.Y)))
                .ToArray();
            
            directlyDetectableAsteroidsLocations = 0;
            orderedAsteroidsGroups = null;

            var result = (0, 0);

            foreach (var location in asteroidsLocations)
            {
                var currentOrderedAsteroidsGroups = asteroidsLocations
                    .Where(l => l != location)
                    .GroupBy(l => Normalize(Subtract(l, location)), new VectorsComparer())
                    .Select(g => g.OrderBy(l => GetLength(Subtract(l, location))).ToArray())
                    .ToArray();

                if (currentOrderedAsteroidsGroups.Length > directlyDetectableAsteroidsLocations)
                {
                    directlyDetectableAsteroidsLocations = currentOrderedAsteroidsGroups.Length;
                    orderedAsteroidsGroups = currentOrderedAsteroidsGroups;
                    result = location;
                }
            }

            return result;
        }

        private static double GetLength((int X, int Y) vector) => Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);

        private static (double X, double Y) Normalize((int X, int Y) vector) => (vector.X / GetLength(vector), vector.Y / GetLength(vector));

        private static (int X, int Y) Subtract((int X, int Y) vector1, (int X, int Y) vector2) => (vector1.X - vector2.X, vector2.Y - vector1.Y);

        private static double GetAngle((int X, int Y) vector)
        {
            if (vector.X == 0)
                return vector.Y >= 0 ? 0 : Math.PI;

            if (vector.Y == 0)
                return vector.X >= 0 ? Math.PI / 2 : Math.PI * 1.5;

            var angle = Math.Atan((double)vector.Y / vector.X);
            if (angle > 0)
                return vector.X > 0 ? Math.PI / 2 - angle : Math.PI * 1.5 - angle;
            if (angle < 0)
                return vector.X < 0 ? Math.PI * 1.5 - angle : Math.PI / 2 - angle;

            return 0;
        }
    }
}
