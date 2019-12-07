using Aoc2019Net.Common;

namespace Aoc2019Net.Days
{
    public sealed class Day2 : Day
    {
        private const string InputTokensDelimiter = ",";

        public override object SolvePart1() => CalculateResult(GetInputNumbers(InputTokensDelimiter), noun: 12, verb: 2);

        public override object SolvePart2()
        {
            const int expectedResult = 19690720;
            const int minNounVerb = 0;
            const int maxNounVerb = 99;

            int result = 0, noun, verb = 0;

            for (noun = minNounVerb; noun <= maxNounVerb && result != expectedResult; noun++)
            {
                for (verb = minNounVerb; verb <= maxNounVerb && result != expectedResult; verb++)
                {
                    var numbers = GetInputNumbers(InputTokensDelimiter);
                    result = CalculateResult(numbers, noun, verb);
                }
            }

            return 100 * (noun - 1) + (verb - 1);
        }

        private static int CalculateResult(int[] numbers, int noun, int verb)
        {
            numbers[1] = noun;
            numbers[2] = verb;
            IntcodeComputer.ExecuteProgram(numbers, new IntcodeComputerParameters());
            return numbers[0];
        }
    }
}
