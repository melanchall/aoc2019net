namespace Aoc2019Net.Tests.Attributes
{
    public sealed class DayDataPart2Attribute : DayDataAttribute
    {
        public DayDataPart2Attribute(string input, object solution)
            : base(input, solution)
        {
        }

        public DayDataPart2Attribute(object solution)
            : this(null, solution)
        {
        }
    }
}
