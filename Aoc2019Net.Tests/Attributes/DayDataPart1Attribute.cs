namespace Aoc2019Net.Tests.Attributes
{
    public sealed class DayDataPart1Attribute : DayDataAttribute
    {
        public DayDataPart1Attribute(string input, object solution)
            : base(input, solution)
        {
        }

        public DayDataPart1Attribute(object solution)
            : this(null, solution)
        {
        }
    }
}
