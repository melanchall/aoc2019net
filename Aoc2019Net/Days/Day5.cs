using Aoc2019Net.Common;
using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day5 : Day
    {
        private const string InputTokensDelimiter = ",";

        public override object SolvePart1() => GetDiagnosticCode(1);

        public override object SolvePart2() => GetDiagnosticCode(5);

        private int GetDiagnosticCode(int inputId)
        {
            var numbers = GetInputNumbers(InputTokensDelimiter);
            var result = IntcodeComputer.ExecuteProgram(numbers, new IntcodeComputerParameters
            {
                Inputs = new[] { inputId }
            });
            return result.Outputs.Last();
        }
    }
}
