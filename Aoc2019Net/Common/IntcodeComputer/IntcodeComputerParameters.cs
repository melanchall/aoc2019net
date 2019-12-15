using System;

namespace Aoc2019Net.Common
{
    internal sealed class IntcodeComputerParameters
    {
        public int[] Inputs { get; set; } = new int[0];

        public Func<int> GetInputValue { get; set; }

        public Action<long> OnOutput { get; set; }

        public int StartIndex { get; set; }

        public bool BreakOnOutput { get; set; }

        public int InputIndex { get; set; }

        public bool ExtendProgram { get; set; }
    }
}
