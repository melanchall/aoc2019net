using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2019Net.Days
{
    public sealed class Day14 : Day
    {
        private sealed class Chemical
        {
            public Chemical(long quantity, string name)
            {
                Quantity = quantity;
                Name = name;
            }

            public long Quantity { get; }

            public string Name { get; }

            public override string ToString()
            {
                return $"{Quantity} {Name}";
            }
        }

        private const string OreChemicalName = "ORE";
        private const string FuelChemicalName = "FUEL";

        public override object SolvePart1()
        {
            var reactions = GetReactions();
            return GetOre(reactions, 1);
        }

        public override object SolvePart2()
        {
            const long oreLimit = 1000000000000;

            var reactions = GetReactions();
            var fuelMin = oreLimit / GetOre(reactions, 1);
            var fuelMax = fuelMin * Enumerable.Range(0, int.MaxValue).First(i => GetOre(reactions, fuelMin * i) > oreLimit);
            var oreMax = GetOre(reactions, fuelMax);

            while (fuelMax - fuelMin > 1)
            {
                oreMax = GetOre(reactions, fuelMax);
                if (oreMax == oreLimit)
                    break;

                var newFuelMax = (fuelMin + fuelMax) / 2;
                var newOreMax = GetOre(reactions, newFuelMax);
                if (newOreMax < oreLimit)
                    fuelMin = newFuelMax;
                else
                    fuelMax = newFuelMax;
            }

            return fuelMin;
        }

        private long GetOre(IEnumerable<(Chemical[] Inputs, Chemical Output)> reactions, long fuel)
        {
            var requiredChemicals = new List<Chemical> { new Chemical(fuel, FuelChemicalName) };
            var chemicalsStock = reactions.ToDictionary(r => r.Output.Name, r => 0L);
            var result = 0L;

            while (requiredChemicals.Any())
            {
                var newRequiredChemicals = new List<Chemical>();

                foreach (var chemical in requiredChemicals)
                {
                    if (chemical.Name == OreChemicalName)
                    {
                        result += chemical.Quantity;
                        continue;
                    }

                    var reactionToProduceChemical = reactions.FirstOrDefault(r => r.Output.Name == chemical.Name);
                    var multiplier = (long)Math.Ceiling((double)chemical.Quantity / reactionToProduceChemical.Output.Quantity);

                    newRequiredChemicals.AddRange(reactionToProduceChemical.Inputs.Select(input => new Chemical(input.Quantity * multiplier, input.Name)));
                    chemicalsStock[chemical.Name] += reactionToProduceChemical.Output.Quantity * multiplier - chemical.Quantity;
                }

                requiredChemicals = newRequiredChemicals
                    .GroupBy(chemical => chemical.Name)
                    .Select(chemicalsGroup =>
                    {
                        var chemicalQuantity = chemicalsGroup.Sum(chemical => chemical.Quantity);
                        var chemicalName = chemicalsGroup.Key;

                        chemicalsStock.TryGetValue(chemicalName, out var inStockQuantity);

                        if (inStockQuantity > 0)
                        {
                            var newQuantity = Math.Max(0, chemicalQuantity - inStockQuantity);
                            chemicalsStock[chemicalName] -= chemicalQuantity - newQuantity;
                            chemicalQuantity = newQuantity;
                        }

                        return new Chemical(chemicalQuantity, chemicalName);
                    })
                    .ToList();
            }

            return result;
        }

        private IEnumerable<(Chemical[] Inputs, Chemical Output)> GetReactions() =>
            (from l in GetInputLines()
             let m = Regex.Match(l, @"(?<i>(?<q>\d+) (?<n>\w+)(, )?)+ => (?<oq>\d+) (?<on>\w+)")
             where m.Success
             select
             (
                 Inputs: Enumerable.Range(0, m.Groups["i"].Captures.Count)
                                   .Select(i => new Chemical(long.Parse(m.Groups["q"].Captures[i].Value), m.Groups["n"].Captures[i].Value))
                                   .ToArray(),
                 Output: new Chemical(long.Parse(m.Groups["oq"].Value), m.Groups["on"].Value)
             ))
             .ToArray();
    }
}
