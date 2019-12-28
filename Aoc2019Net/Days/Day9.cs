using Aoc2019Net.Common;
using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day9 : Day
    {
        public override object SolvePart1() => GetOuput(1);

        public override object SolvePart2() => GetOuput(2);

        private long GetOuput(int input)
        {
            var program = GetInputIntcodeProgram();

            var result = IntcodeComputer.ExecuteProgram(program, new IntcodeComputerParameters
            {
                Inputs = new[] { input },
                ExtendProgram = true
            });

            return result.Outputs.Last();
        }
    }
}
