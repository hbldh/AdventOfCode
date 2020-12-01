using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
   
    class Day1 : IDay
    {

        private readonly string data;
        private readonly List<int> values;

        public Day1()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input01.txt"));
            this.values = this.data.Split('\n',options:StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(int.Parse);
        }

        public string SolvePartOne()
        {
            return this.values.Select(v => CalculateMass(v)).Sum().ToString();
        }

        public string SolvePartTwo()
        {
            return this.values.Select(v => RecursiveCalculateMass(v)).Sum().ToString();
        }


        private int CalculateMass(int v)
        {
                return Convert.ToInt32(Math.Floor((double)v / 3)) - 2;
        }
        private int RecursiveCalculateMass(int v)
        {
            int r = Convert.ToInt32(Math.Floor((double)v / 3)) - 2;
            if (r > 0)
            {
                return r + RecursiveCalculateMass(r);
            }
            else
            {
                return 0;
            }
        }
    }
}
