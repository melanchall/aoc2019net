namespace Aoc2019Net.Common
{
    internal sealed class IntcodeComputerResult
    {
        public IntcodeComputerResult(long[] outputs, bool halted, int stoppedAtIndex, int newInputIndex)
        {
            Outputs = outputs;
            Halted = halted;
            StoppedAtIndex = stoppedAtIndex;
            NewInputIndex = newInputIndex;
        }

        public long[] Outputs { get; }

        public bool Halted { get; }

        public int StoppedAtIndex { get; }

        public int NewInputIndex { get; }
    }
}
