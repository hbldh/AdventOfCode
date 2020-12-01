using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day4 : IDay
    {
        private readonly string data;
        private Tuple<int, int> limits;

        public Day4()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input04.txt"));
            this.limits = new Tuple<int, int>(int.Parse(this.data.Split('-')[0]), int.Parse(this.data.Split('-')[1]));
        }

        public string SolvePartOne()
        {
            List<int> passwords = new List<int>();
            List<int> parts;
            for (int candidate = this.limits.Item1; candidate <= this.limits.Item2; candidate++)
            {
                parts = candidate.ToString().ToList().Select( c => c.ToString()).ToList().ConvertAll(int.Parse);
                if (parts.Distinct().Count() < 6 && parts.SequenceEqual(parts.OrderBy(o => o)))
                {
                    passwords.Add(candidate);
                }
            }
            return passwords.Count.ToString();
        }

        public string SolvePartTwo()
        {
            List<int> passwords = new List<int>();
            List<int> parts;
            for (int candidate = this.limits.Item1; candidate <= this.limits.Item2; candidate++)
            {
                parts = candidate.ToString().ToList().Select( c => c.ToString()).ToList().ConvertAll(int.Parse);
                if (parts.SequenceEqual(parts.OrderBy(o => o)) && parts.GroupBy(o => o).Select(o => o.Count()).Contains(2))
                {
                    passwords.Add(candidate);
                }
            }
            return passwords.Count.ToString();
        }
    }
}
