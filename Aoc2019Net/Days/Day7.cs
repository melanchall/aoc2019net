using Aoc2019Net.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day7 : Day
    {
        private sealed class AmplifierState
        {
            public int[] Program { get; set; }

            public List<int> Inputs { get; } = new List<int>();

            public bool Halted { get; set; }

            public int InputIndex { get; set; }

            public int StartIndex { get; set; }

            public int LastOutput { get; set; }
        }

        private const string InputTokensDelimiter = ",";
        private const int AmplifiersCount = 5;

        public override object SolvePart1()
        {
            var program = GetInputNumbers(InputTokensDelimiter);

            var phaseSettingsSets = GetPhaseSettingsSets(new int[0], new[] { 0, 1, 2, 3, 4 }).ToArray();
            var maxOutput = 0;

            foreach (var phaseSettings in phaseSettingsSets)
            {
                var lastOutput = 0;

                for (var i = 0; i < AmplifiersCount; i++)
                {
                    var result = IntcodeComputer.ExecuteProgram(program.ToArray(), new IntcodeComputerParameters
                    {
                        Inputs = new[] { phaseSettings[i], lastOutput }
                    });
                    lastOutput = result.Outputs.Last();
                }

                maxOutput = Math.Max(maxOutput, lastOutput);
            }

            return maxOutput;
        }

        public override object SolvePart2()
        {
            var program = GetInputNumbers(InputTokensDelimiter);

            var phaseSettingsSets = GetPhaseSettingsSets(new int[0], new[] { 5, 6, 7, 8, 9 }).ToArray();
            var maxOutput = 0;

            foreach (var phaseSettings in phaseSettingsSets)
            {
                var states = Enumerable.Range(0, AmplifiersCount)
                    .Select(i => new AmplifierState { Program = program.ToArray() })
                    .ToArray();

                while (states.Any(s => !s.Halted))
                {
                    for (var i = 0; i < AmplifiersCount; i++)
                    {
                        var state = states[i];

                        if (!state.Inputs.Any())
                            state.Inputs.Add(phaseSettings[i]);

                        state.Inputs.Add(i == 0 ? states.Last().LastOutput : states[i - 1].LastOutput);

                        var result = IntcodeComputer.ExecuteProgram(
                            state.Program,
                            new IntcodeComputerParameters
                            {
                                BreakOnOutput = true,
                                Inputs = state.Inputs.ToArray(),
                                StartIndex = state.StartIndex,
                                InputIndex = state.InputIndex
                            });

                        if (result.Halted)
                        {
                            state.Halted = true;
                            continue;
                        }

                        state.LastOutput = result.Outputs.Last();
                        state.StartIndex = result.StoppedAtIndex;
                        state.InputIndex = result.NewInputIndex;
                    }
                }

                maxOutput = Math.Max(maxOutput, states.Last().LastOutput);
            }

            return maxOutput;
        }

        private static IEnumerable<int[]> GetPhaseSettingsSets(int[] currentSet, int[] validPhaseSettings)
        {
            if (currentSet.Length == validPhaseSettings.Length)
                return new[] { currentSet };

            return from phaseSettings in validPhaseSettings.Where(y => !currentSet.Contains(y))
                   from set in GetPhaseSettingsSets(currentSet.Concat(new[] { phaseSettings }).ToArray(), validPhaseSettings)
                   select set;
        }
    }
}
