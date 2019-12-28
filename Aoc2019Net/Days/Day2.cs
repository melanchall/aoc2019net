using Aoc2019Net.Common;

namespace Aoc2019Net.Days
{
    public sealed class Day2 : Day
    {
        public override object SolvePart1() => CalculateResult(GetInputIntcodeProgram(), noun: 12, verb: 2);

        public override object SolvePart2()
        {
            const int expectedResult = 19690720;
            const int minNounVerb = 0;
            const int maxNounVerb = 99;

            long result = 0, noun, verb = 0;

            for (noun = minNounVerb; noun <= maxNounVerb && result != expectedResult; noun++)
            {
                for (verb = minNounVerb; verb <= maxNounVerb && result != expectedResult; verb++)
                {
                    var program = GetInputIntcodeProgram();
                    result = CalculateResult(program, noun, verb);
                }
            }

            return 100 * (noun - 1) + (verb - 1);
        }

        private static long CalculateResult(long[] program, long noun, long verb)
        {
            program[1] = noun;
            program[2] = verb;
            IntcodeComputer.ExecuteProgram(program, new IntcodeComputerParameters());
            return program[0];
        }
    }
}
