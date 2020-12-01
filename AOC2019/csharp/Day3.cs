using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{

    class TupleComparer : IEqualityComparer<Tuple<int, int, int>>
    {
        public bool Equals(Tuple<int, int, int> x, Tuple<int, int, int> y)
        {
            return x.Item1 == y.Item1 && x.Item2 == y.Item2;
        }

        public int GetHashCode(Tuple<int, int, int> product)
        {
            return product.Item1.GetHashCode() ^ product.Item2.GetHashCode();
        }
    }

    class Day3 : IDay
    {
        private readonly string data;
        private List<string> firstPath;
        private List<string> secondPath;

        public Day3()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input03.txt"));
            this.firstPath = this.data.Split('\n')[0].Split(',', options:StringSplitOptions.RemoveEmptyEntries).ToList();
            this.secondPath = this.data.Split('\n')[1].Split(',', options:StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public string SolvePartOne()
        {
            var first = FollowPath(this.firstPath);
            var second = FollowPath(this.secondPath);
            var first2 = first.Select(o => new Tuple<int,int>(o.Item1, o.Item2)).ToList();
            var second2 = second.Select(o => new Tuple<int,int>(o.Item1, o.Item2)).ToList();
            var intersections = first2.Intersect(second2).OrderBy(o => Math.Abs(o.Item1) + Math.Abs(o.Item2)).ToList();
            return (Math.Abs(intersections[0].Item1) + Math.Abs(intersections[0].Item2)).ToString();
        }

        private List<Tuple<int, int, int>> FollowPath(List<string> pathSpec)
        {
            int y = 0;
            int x = 0;
            int n = 0;
            List<Tuple<int, int, int>> path = new List<Tuple<int, int, int>>();
            foreach (var p in pathSpec)
            {
                switch (p[0])
                {
                    case 'U':
                        for (int i = 0; i < int.Parse(p.Substring(1)); i++)
                        {
                            y += 1;
                            n += 1;
                            path.Add(new Tuple<int, int, int>(x,y,n));
                        }
                        break;
                    case 'D':
                        for (int i = 0; i < int.Parse(p.Substring(1)); i++)
                        {
                            y -= 1;
                            n += 1;
                            path.Add(new Tuple<int, int, int>(x,y,n));
                        }
                        break;
                    case 'R':
                        for (int i = 0; i < int.Parse(p.Substring(1)); i++)
                        {
                            x += 1;
                            n += 1;
                            path.Add(new Tuple<int, int, int>(x,y,n));
                        }
                        break;
                    case 'L':
                        for (int i = 0; i < int.Parse(p.Substring(1)); i++)
                        {
                            x -= 1;
                            n += 1;
                            path.Add(new Tuple<int, int, int>(x,y,n));
                        }
                        break;
                }
            }
            return path;
        }

        public string SolvePartTwo()
        {
            var first = FollowPath(this.firstPath);
            var second = FollowPath(this.secondPath);
            // var intersections = new List<Tuple<int, int, int>>();
            var intersections = first.Intersect(second, new TupleComparer());
            var dists = new List<int>();
            foreach(var intersection in intersections)
            {
                foreach(var f in first.Where(o=> o.Item1 == intersection.Item1 && o.Item2 == intersection.Item2))
                {
                    foreach(var s in second.Where(o=> o.Item1 == intersection.Item1 && o.Item2 == intersection.Item2))
                    {
                        dists.Add(f.Item3 + s.Item3);
                    }
                }
            }
            return dists.Min().ToString();
        }
    }
}
