namespace Aoc2019Net.Common
{
    internal sealed class IntcodeComputerParameters
    {
        public int[] Inputs { get; set; } = new int[0];

        public int StartIndex { get; set; }

        public bool BreakOnOutput { get; set; }

        public int InputIndex { get; set; }
    }
}
