using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day10 : IDay
    {
        private readonly string data;
        private List<List<bool>> astroidMap;
        private HashSet<Tuple<int, int>> asteroidSet;

        public Day10()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input10.txt"));
            this.astroidMap = this.data
                .Trim()
                .Split('\n',options:StringSplitOptions.RemoveEmptyEntries)
                .Select(row => row.ToCharArray()
                    .Select(cell => cell == '#')
                    .ToList()
                )
                .ToList();
            this.asteroidSet = new HashSet<Tuple<int, int>>();
            for (int col=0; col < this.astroidMap[0].Count; col ++)
            {
                for (int row=0; row < this.astroidMap.Count; row ++)
                {
                    if (this.astroidMap[row][col])
                    {
                        this.asteroidSet.Add(new Tuple<int, int>(col, row));
                    }
                }
            }
        }

        public string SolvePartOne()
        {
            var nObservable = new Dictionary<Tuple<int, int>, int>();
            foreach (var ap in this.asteroidSet)
            {
                int n = 0;
                foreach (var ap2 in this.asteroidSet.Where(x => x != ap))
                {
                    //List<Tuple<int, int>> line = GetIntegerPointsOnLine(ap, ap2));

                }
            }
            /*
            n_observable = {}
            for ap in astroid_dict:
                n = 0
                for ap2 in filter(lambda x: x != ap, astroid_dict):
                    line = list(get_line(ap, ap2))
                    x = [astroid_dict.get(v, False) for v in line]
                    n += int(not any(x[1:-1]))
                n_observable[ap] = n
            g = sorted(n_observable.items(), key=lambda x: x[1], reverse=True)
            return g[0][1], g[0][0]  # Value first, position after that, for part 2.
            */
            return "";
        }

        private List<Tuple<int, int>> GetIntegerPointsOnLine(Tuple<int, int> start, Tuple<int, int> end)
        {
            if (start.Item1 == end.Item1)
            {
                return this.GetYRange(start, end).Select(y => new Tuple<int, int>(end.Item1, y)).ToList();
            }
            if (start.Item2 == end.Item2)
            {
                return this.GetXRange(start, end).Select(x => new Tuple<int, int>(x, end.Item2)).ToList();
            }
            var kNumerator = end.Item2 - start.Item2;
            var kDenominator = end.Item1 - start.Item1;
            kNumerator = Math.Sign(kNumerator / kDenominator) * kNumerator;
            kDenominator = Math.Abs(kDenominator);
            var sign = end.Item1 < start.Item2 ? -1 : 1;
            
            /*
            k = fractions.Fraction((end[1] - start[1]), (end[0] - start[0]))
            k_num = k.numerator
            k_denom = k.denominator

            sign = -1 if end[0] < start[0] else 1
            x_step = sign * k_denom
            y_step = sign * k_num

            return (
                (x, y)
                for x, y in zip(
                    range(start[0], end[0] + (1 if start[0] < end[0] else -1), x_step),
                    range(start[1], end[1] + (1 if start[1] < end[1] else -1), y_step),
                )
            )
            */
            throw new NotImplementedException();
        }

        private IEnumerable<int> GetXRange(Tuple<int, int> start, Tuple<int, int> end)
        {
            if (start.Item1 < end.Item1)
            {
                return Enumerable.Range(start.Item1, end.Item1 - start.Item1 + 1);
            }
            else
            {
                return Enumerable.Range(end.Item1, start.Item1 - end.Item1 + 1).Reverse();
            }
        }

        private IEnumerable<int> GetYRange(Tuple<int, int> start, Tuple<int, int> end)
        {
            if (start.Item2 < end.Item2)
            {
                return Enumerable.Range(start.Item2, end.Item2 - start.Item2 + 1);
            }
            else
            {
                return Enumerable.Range(end.Item2, start.Item2 - end.Item2 + 1).Reverse();
            }
        }

        public string SolvePartTwo()
        {
            return "";
        }
        
    }
}
