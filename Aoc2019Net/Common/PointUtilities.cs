using System.Collections.Generic;

namespace Aoc2019Net.Common
{
    public static class PointUtilities
    {
        public static IEnumerable<(int X, int Y)> GetNearestLocations(this (int X, int Y) point) => new[]
        {
            (point.X, point.Y + 1),
            (point.X, point.Y - 1),
            (point.X - 1, point.Y),
            (point.X + 1, point.Y)
        };
    }
}
