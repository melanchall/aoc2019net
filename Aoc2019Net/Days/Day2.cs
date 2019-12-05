using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day2 : Day
    {
        private const string InputTokensDelimiter = ",";

        protected override object SolvePart1() => CalculateResult(GetInputNumbers(InputTokensDelimiter), noun: 12, verb: 2);

        protected override object SolvePart2()
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
            const int addCommand = 1;
            const int multiplyCommand = 2;
            const int haltCommand = 99;

            numbers[1] = noun;
            numbers[2] = verb;

            for (var i = 0; i < numbers.Length; i += 4)
            {
                var command = numbers[i];
                if (command == haltCommand)
                    break;

                var xPosition = numbers[i + 1];
                var yPosition = numbers[i + 2];
                var resultPosition = numbers[i + 3];

                switch (command)
                {
                    case addCommand:
                        numbers[resultPosition] = numbers[xPosition] + numbers[yPosition];
                        break;
                    case multiplyCommand:
                        numbers[resultPosition] = numbers[xPosition] * numbers[yPosition];
                        break;
                }
            }

            return numbers[0];
        }
    }
}
