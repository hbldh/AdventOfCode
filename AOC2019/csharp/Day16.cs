using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day16 : IDay
    {
        private readonly string data;
        private readonly int[] values;

        public Day16()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            //this.data = "12345678";
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input16.txt"));
            this.values = this.data.Trim().ToCharArray().Select(x => int.Parse(x.ToString())).ToArray();
        }

        public string SolvePartOne()
        {
            var fft = values;
            for (int i = 0; i < 100; i++)
            {
                fft  = fft.Select((x, pos) => this.GenerateOutputValue(fft, pos + 1)).ToArray();
            }
            return string.Concat(fft.Take(8).Select(x => x.ToString()));
        }

        private int GenerateOutputValue(int[] x, int position)
        {   
            List<int> mask = new List<int>();
            int[] components = new int[4] {0,1,0,-1};
            while (mask.Count <= x.Length)
            {
                foreach (var n in components)
                {
                    mask.AddRange((new int[position]).Select(o => n));
                }
            }

            int y = Math.Abs(x.Zip(mask.Skip(1).Take(x.Length), (a,b) => a * b).Sum());
            return  y % 10;
        }

        public string SolvePartTwo()
        {
            var offset = int.Parse(data.Substring(0,7));
            var length = (10000 * this.values.Length) - offset;
            var digits = this.values.Reverse().ToArray();
            var longlist = digits.ToList();
            while (longlist.Count < length)
            {
                longlist.AddRange(digits);
            }
            var longarray = longlist.Take(length).ToArray();
            var vals = new List<int>();
            
            for (int i = 0; i < 100; i++)
            {
                var cumulative = new int[longarray.Length];
                for (int j = 0; j < longarray.Length; j++)
                {
                    cumulative[j] = longarray[j] + (j == 0 ? 0 : cumulative[j-1]);
                }
                longarray = cumulative.Select(n => n % 10).ToArray();
            }
            return String.Join("", longarray.Reverse().Take(8));
        }
    }

    
}
