using Aoc2019Net.Common;
using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day9 : Day
    {
        private const string InputTokensDelimiter = ",";

        public override object SolvePart1() => GetOuput(1);

        public override object SolvePart2() => GetOuput(2);

        private long GetOuput(int input)
        {
            var numbers = GetInputNumbers(InputTokensDelimiter);

            var result = IntcodeComputer.ExecuteProgram(numbers, new IntcodeComputerParameters
            {
                Inputs = new[] { input },
                ExtendProgram = true
            });

            return result.Outputs.Last();
        }
    }
}
