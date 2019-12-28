using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2019Net.Days
{
    public sealed class Day12 : Day
    {
        private const string XGroupName = "x";
        private const string YGroupName = "y";
        private const string ZGroupName = "z";
        private static readonly Regex InputRegex = new Regex(@$"<x=(?<{XGroupName}>\-?\d+), y=(?<{YGroupName}>\-?\d+), z=(?<{ZGroupName}>\-?\d+)>");

        public override object SolvePart1()
        {
            var (positions, velocities) = GetInitialState();
            var steps = GetFirstParameter(1000);

            for (int s = 0; s < steps; s++)
            {
                Move(positions, velocities);
            }

            return Enumerable.Range(0, 4).Select(i => positions[i].Sum(Math.Abs) * velocities[i].Sum(Math.Abs)).Sum();
        }

        public override object SolvePart2()
        {
            return null;
        }

        private (int[][] Positions, int[][] Velocities) GetInitialState()
        {
            var positions = (from line in GetInputLines()
                             let m = InputRegex.Match(line)
                             where m.Success
                             let x = int.Parse(m.Groups[XGroupName].Value)
                             let y = int.Parse(m.Groups[YGroupName].Value)
                             let z = int.Parse(m.Groups[ZGroupName].Value)
                             select new[] { x, y, z }).ToArray();
            var velocities = positions.Select(_ => new int[3]).ToArray();

            return (positions, velocities);
        }

        private static void Move(int[][] positions, int[][] velocities)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                var pi = positions[i];

                for (int j = i; j < positions.Length; j++)
                {
                    var pj = positions[j];

                    for (int k = 0; k < 3; k++)
                    {
                        if (pi[k] == pj[k])
                            continue;

                        var m = pi[k] < pj[k] ? i : j;
                        var n = pi[k] < pj[k] ? j : i;

                        velocities[m][k]++;
                        velocities[n][k]--;
                    }
                }
            }

            for (int i = 0; i < positions.Length; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    positions[i][k] += velocities[i][k];
                }
            }
        }
    }
}
