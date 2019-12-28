using Aoc2019Net.Common;
using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day5 : Day
    {
        public override object SolvePart1() => GetDiagnosticCode(1);

        public override object SolvePart2() => GetDiagnosticCode(5);

        private long GetDiagnosticCode(int inputId)
        {
            var program = GetInputIntcodeProgram();
            var result = IntcodeComputer.ExecuteProgram(program, new IntcodeComputerParameters
            {
                Inputs = new[] { inputId }
            });
            return result.Outputs.Last();
        }
    }
}
