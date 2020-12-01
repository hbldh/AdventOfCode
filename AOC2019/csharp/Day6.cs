using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day6 : IDay
    {
        private readonly string data;
        private readonly List<Tuple<string, string>> orbits;

        public Day6()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input06.txt"));
            this.orbits = this.data.Trim().Split('\n').Select(o => new Tuple<string, string>(o.Split(')')[0], o.Split(')')[1])).ToList();
        }

        public string SolvePartOne()
        {
            var nodes = new HashSet<string>();
            var connections = new Dictionary<string, string>();
            foreach (var orbit in this.orbits)
            {
                nodes.Add(orbit.Item1);
                nodes.Add(orbit.Item2);
                connections.Add(orbit.Item2, orbit.Item1);
            }
            int sum = 0;
            string next;
            foreach (var node in nodes)
            {
                next = node;
                while (true)
                {
                    if (next == "COM")
                    {
                        break;
                    } 
                    next = connections[next];
                    sum += 1;
                }
            }

            return sum.ToString();
        }
        
        public string SolvePartTwo()
        {
            var nodes = new HashSet<string>();
            var connections = new Dictionary<string, string>();
            foreach (var orbit in this.orbits)
            {
                nodes.Add(orbit.Item1);
                nodes.Add(orbit.Item2);
                connections.Add(orbit.Item2, orbit.Item1);
            }

            string next = "YOU";
            var YOUPath = new List<string>();
            var SANPath = new List<string>();
            while (true)
            {
                if (next == "COM")
                {
                    break;
                } 
                next = connections[next];
                YOUPath.Add(next);
            }

            next = "SAN";
            string common = "";
            while (true)
            {
                if (next == "COM")
                {
                    break;
                } 
                next = connections[next];
                SANPath.Add(next);
                if (YOUPath.Contains(next))
                {
                    common = next;
                    break;
                }
            }

            next = "YOU";
            int sum = SANPath.Count - 1;

            while (true)
            {
                if (next == common)
                {
                    break;
                } 
                next = connections[next];
                sum += 1;
            }

            return (sum - 1).ToString();
        }
    }
}
