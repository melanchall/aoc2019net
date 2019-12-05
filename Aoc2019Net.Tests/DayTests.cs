﻿using Aoc2019Net.Tests.Attributes;
using NUnit.Framework;
using System;
using System.Linq;

namespace Aoc2019Net.Tests
{
    public abstract class DayTests<T>
    {
        public TestContext TestContext { get; set; }

        [Test]
        [TestCaseSource(nameof(GetDayDataPart1))]
        public void SolvePart1(string input, object expectedSolution)
        {
            var solution = GetDaySolution(input);
            Assert.AreEqual(expectedSolution, solution.Part1, "Solution is wrong.");
        }

        [Test]
        [TestCaseSource(nameof(GetDayDataPart2))]
        public void SolvePart2(string input, object expectedSolution)
        {
            var solution = GetDaySolution(input);
            Assert.AreEqual(expectedSolution, solution.Part2, "Solution is wrong.");
        }

        public static object[] GetDayDataPart1() => GetDayData<DayDataPart1Attribute>();

        public static object[] GetDayDataPart2() => GetDayData<DayDataPart2Attribute>();

        private static object[] GetDayData<TAttribute>()
            where TAttribute : DayDataAttribute
        {
            return (from a in Attribute.GetCustomAttributes(typeof(T), typeof(TAttribute))
                    let dayDataAttribute = (TAttribute)a
                    select new object[] { dayDataAttribute.Input, dayDataAttribute.Solution }).ToArray();
        }

        private (object Part1, object Part2) GetDaySolution(string input)
        {
            var className = GetType().Name;
            var testsWordIndex = className.IndexOf("Tests");
            var dayClassName = className.Substring(0, testsWordIndex);
            var dayType = typeof(Day).Assembly.GetTypes().FirstOrDefault(t => t.Name == dayClassName);
            var day = (Day)Activator.CreateInstance(dayType);

            if (!string.IsNullOrWhiteSpace(input))
                day.Input = input;

            return day.Solve();
        }
    }
}
