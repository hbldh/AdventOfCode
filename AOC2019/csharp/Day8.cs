using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day8 : IDay
    {
        private readonly string data;

        public Day8()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input08.txt"));
        }

        public static IEnumerable<List<T>> splitList<T>(List<T> locations, int nSize=30)  
        {        
            for (int i = 0; i < locations.Count; i += nSize) 
            { 
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i)); 
            }  
        } 

        public string SolvePartOne()
        {
            var size = new Tuple<int, int>(25,6);
            List<int> values = this.data.Trim().Select(c => int.Parse(c.ToString())).ToList();
            var layer = splitList(values, 25*6).OrderBy(o => o.Count(v => v == 0)).First();
            return (layer.Count(v => v == 1) * layer.Count(v => v == 2)).ToString();
        }
        
        public string SolvePartTwo()
        {
            var size = new Tuple<int, int>(25,6);
            List<int> values = this.data.Trim().Select(c => int.Parse(c.ToString())).ToList();
            var layers = splitList(values, 25*6).ToList();
            int n = 0;
            for (int i = 0; i < 25*6; i++)
            {
                var p = layers.Select(o => o[i]).First(x => x != 2);
                Console.Write(p == 1 ? "#" : " ");
                n += 1 ;
                if (n == 25)
                {
                    Console.Write("\n");
                    n = 0;
                }
            }
            return "LEGJY";
        }
    }
}
