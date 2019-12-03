using System;

namespace Aoc2019Net.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public abstract class DayDataAttribute : Attribute
    {
        public DayDataAttribute(string input, object solution)
        {
            Input = input;
            Solution = solution;
        }

        public string Input { get; }

        public object Solution { get; }
    }
}
