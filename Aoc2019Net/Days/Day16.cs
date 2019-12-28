using System;
using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day16 : Day
    {
        public override object SolvePart1()
        {
            var steps = GetFirstParameter(100);
            var signal = GetInput().Select(c => int.Parse(c.ToString())).ToArray();

            var basePattern = new[] { 0, 1, 0, -1 };
            var patterns = signal.Select((_, i) => Enumerable.Range(0, int.MaxValue)
                                                             .SelectMany(_ => basePattern.SelectMany(nn => Enumerable.Range(0, i + 1).Select(_ => nn)))
                                                             .Skip(1)
                                                             .Take(signal.Length)
                                                             .ToArray())
                                 .ToArray();

            var tmp = new int[signal.Length];

            for (var s = 0; s < steps; s++)
            {
                for (var i = 0; i < signal.Length; i++)
                {
                    var sum = 0;

                    for (var k = i; k < signal.Length; k += 4 * (i + 1))
                    {
                        for (var j = 0; j < i + 1 && k + j < signal.Length; j++)
                        {
                            sum += signal[k + j];
                        }
                    }

                    for (var k = (i + 1) * 3 - 1; k < signal.Length; k += 4 * (i + 1))
                    {
                        for (var j = 0; j < i + 1 && k + j < signal.Length; j++)
                        {
                            sum -= signal[k + j];
                        }
                    }

                    tmp[i] = Math.Abs(sum % 10);
                }

                var t = signal;
                signal = tmp;
                tmp = t;
            }

            return string.Join(string.Empty, signal.Take(8));
        }

        public override object SolvePart2()
        {
            return null;
        }
    }
}
