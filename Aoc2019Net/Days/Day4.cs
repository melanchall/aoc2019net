using System.Linq;

namespace Aoc2019Net.Days
{
    public sealed class Day4 : Day
    {
        protected override object SolvePart1() => Solve(largeGroupsForbidden: false);

        protected override object SolvePart2() => Solve(largeGroupsForbidden: true);

        private object Solve(bool largeGroupsForbidden)
        {
            var boundsStrings = GetInputLines().First().Split("-");
            var min = int.Parse(boundsStrings[0]);
            var max = int.Parse(boundsStrings[1]);

            return Enumerable
                .Range(min, max - min + 1)
                .Count(i => IsValidPassword(i.ToString(), largeGroupsForbidden));
        }

        private static bool IsValidPassword(string password, bool largeGroupsForbidden)
        {
            var lastChar = (char)('0' - 1);
            var duplicateCount = 0;
            var isValidPassword = false;

            foreach (var c in password + (char)('9' + 1))
            {
                if (c < lastChar)
                    return false;

                if (c == lastChar)
                    duplicateCount++;
                else
                {
                    isValidPassword |= largeGroupsForbidden ? duplicateCount == 1 : duplicateCount > 0;
                    duplicateCount = 0;
                }

                lastChar = c;
            }

            return isValidPassword;
        }
    }
}
