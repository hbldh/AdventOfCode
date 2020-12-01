using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day14 : IDay
    {
        private readonly string data;
        private List<((string name, long qty) output, List<(string name, long qty)> input)> elements;

        public Dictionary<string, long> surplus { get; private set; }

        public Day14()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            //this.data = "157 ORE => 5 NZVS\n165 ORE => 6 DCFZ\n44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL\n12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ\n179 ORE => 7 PSHF\n177 ORE => 5 HKGWZ\n7 DCFZ, 7 PSHF => 2 XJWVT\n165 ORE => 2 GPVTF\n3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT";
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input14.txt"));
            this.elements = this.data.Trim().Split("\n").Select(row => this.ParseToElement(row.Split("=>"))).ToList();
        }

        private ((string name, long qty) output, List<(string name, long qty)> input) ParseToElement(string[] v)
        {
            var produced = this.ParseToElement(v[1]);
            var required = v[0].Split(',').Select(e => this.ParseToElement(e)).ToList();
            return (output: produced, input: required);
        }
        private (string name, long qty) ParseToElement(string element)
        {
            var parts = element.Trim().Split(' ');
            return (name: parts[1], qty:long.Parse(parts[0]));
        }

        public string SolvePartOne()
        {
            return this.Produce("FUEL", 1).ToString();
        }

        public string SolvePartTwo()
        {
            long lo = 0;
            long hi = 1000000000000;
            
            while ((hi - lo) > 1)
            {
                long val = lo + (hi - lo) / 2;
                long oreNeeded = this.Produce("FUEL", val);
                if (oreNeeded > 1000000000000)
                {
                    hi = val;
                }
                else
                {
                    lo = val;
                }
            }
            return lo.ToString();
        }

        private long Produce(string name, long qty)
        {
            var required = new Dictionary<string, long>() {{name, qty}};
            this.surplus = new Dictionary<string, long>();
            long ore = 0;

            while (required.Count > 0)
            {
                var current = required.First();
                required.Remove(current.Key);

                var componentsNeeded = this.Reduce((current.Key, current.Value));
                if (componentsNeeded.ContainsKey("ORE"))
                {
                    ore += componentsNeeded["ORE"];
                    componentsNeeded.Remove("ORE");
                }
                foreach (var component in componentsNeeded)
                {
                    required[component.Key] = required.GetValueOrDefault(component.Key, 0) + component.Value;
                }
            }
            
            return ore;
        }

        private Dictionary<string, long> Reduce((string name, long qty) required)
        {
            var amount = required.qty;
            var thisReaction = this.elements.Where(o => o.output.name == required.name).First();
            var multiples = (long)Math.Ceiling((double) amount / thisReaction.output.qty);
            
            this.surplus[required.name] = this.surplus.GetValueOrDefault(required.name, 0) + (multiples * thisReaction.output.qty) - required.qty;
            Dictionary<string, long> componentsNeeded = new Dictionary<string, long>();
            foreach (var component in thisReaction.input)
            {
                var totalAmountNeeded = component.qty * multiples;
                var usableSurplus = Math.Min(totalAmountNeeded, this.surplus.GetValueOrDefault(component.name, 0));
                totalAmountNeeded -= usableSurplus;
                this.surplus[component.name] = this.surplus.GetValueOrDefault(component.name, 0) - usableSurplus;
                componentsNeeded[component.name] = totalAmountNeeded;
            }
            return componentsNeeded;
        }
    }
}
