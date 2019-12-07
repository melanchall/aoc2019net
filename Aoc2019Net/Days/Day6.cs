using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day6 : Day
    {
        public override object SolvePart1()
        {
            var orbitsConnections = GetOrbitsConnections();
            var directOrbitsCount = orbitsConnections.Keys.Count;
            var indirectOrbitsCount = 0;

            foreach (var connection in orbitsConnections)
            {
                for (var connectedTo = connection.Value; orbitsConnections.TryGetValue(connectedTo, out var next); connectedTo = next)
                {
                    indirectOrbitsCount++;
                }
            }

            return directOrbitsCount + indirectOrbitsCount;
        }

        public override object SolvePart2()
        {
            const string from = "YOU";
            const string to = "SAN";

            var orbitsConnections = GetOrbitsConnections();
            return GetRoutes(Enumerable.Empty<string>(), from, orbitsConnections)
                .Where(r => r.LastOrDefault() == to)
                .Min(r => r.Count - 2);
        }

        private static IEnumerable<ICollection<string>> GetRoutes(IEnumerable<string> currentRoute, string start, Dictionary<string, string> orbitsConnections)
        {
            orbitsConnections.TryGetValue(start, out var connectedTo);
            var newObjects = orbitsConnections
                .Where(c => c.Value == start)
                .Select(c => c.Key)
                .Concat(new[] { connectedTo });

            foreach (var obj in newObjects.Where(o => !string.IsNullOrWhiteSpace(o) && !currentRoute.Contains(o)))
            {
                var newRoute = currentRoute.Concat(new[] { obj }).ToArray();
                yield return newRoute;

                foreach (var route in GetRoutes(newRoute, obj, orbitsConnections))
                {
                    yield return route;
                }
            }
        }

        private Dictionary<string, string> GetOrbitsConnections() =>
            (from l in GetInputLines()
             let delimiterIndex = l.IndexOf(')')
             let x = l[(delimiterIndex + 1)..]
             let y = l[..delimiterIndex]
             select new { X = x, Y = y })
            .ToDictionary(c => c.X, c => c.Y);
    }
}
