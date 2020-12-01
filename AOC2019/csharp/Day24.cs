using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace AOC2019
{
    class Day24 : IDay
    {
        private readonly string data;

        public List<List<string>> map { get; }
        public Dictionary<int, int> intmap { get; }
        public Dictionary<int, List<int>> neighboursPartOne { get; }
        public Dictionary<int, List<int>> neighboursPartTwo { get; }
        public Dictionary<int, List<int>> innerNeighbours { get; }
        public Dictionary<int, List<int>> outerNeighbours { get; }

        public Day24()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input24.txt"));
            //this.data = "....#\n#..#.\n#..##\n..#..\n#....";
            
            this.map = this.data.Trim().Split('\n').Select(x => x.ToCharArray().Select(v => v.ToString()).ToList()).ToList();
            this.intmap = this.map.SelectMany(i => i).Select((v,i) => (v == "#" ? 1: 0,i)).ToDictionary(x => x.i + 1, y=> y.Item1);
            this.neighboursPartOne = new Dictionary<int, List<int>> ()
            {
                {1, new List<int>() {2,6}},
                {2, new List<int>() {1,7,3}},
                {3, new List<int>() {2,8,4}},
                {4, new List<int>() {3,9,5}},
                {5, new List<int>() {4,10}},
                {6, new List<int>() {1,7,11}},
                {7, new List<int>() {6,2,8,12}},
                {8, new List<int>() {3,7,9}},
                {9, new List<int>() {4,8,10,14}},
                {10, new List<int>() {5,9,15}},
                {11, new List<int>() {6,12,16}},
                {12, new List<int>() {7,11,17}},
                {13, new List<int>() {8,12,14,18}},
                {14, new List<int>() {9,15,19}},
                {15, new List<int>() {10,14,20}},
                {16, new List<int>() {11,17,21}},
                {17, new List<int>() {12,16,18,22}},
                {18, new List<int>() {17,19,23}},
                {19, new List<int>() {14,18,20,24}},
                {20, new List<int>() {15,19,25}},
                {21, new List<int>() {16,22}},
                {22, new List<int>() {17,21,23}},
                {23, new List<int>() {18,22,24}},
                {24, new List<int>() {19,23,25}},
                {25, new List<int>() {20,24}}
            };
            this.neighboursPartTwo = new Dictionary<int, List<int>>(this.neighboursPartOne);
            this.neighboursPartTwo[13] = new List<int>();
            this.innerNeighbours = new Dictionary<int, List<int>> ()
            {
                {1, new List<int>() {}},
                {2, new List<int>() {}},
                {3, new List<int>() {}},
                {4, new List<int>() {}},
                {5, new List<int>() {}},
                {6, new List<int>() {}},
                {7, new List<int>() {}},
                {8, new List<int>() {1,2,3,4,5}},
                {9, new List<int>() {}},
                {10, new List<int>() {}},
                {11, new List<int>() {}},
                {12, new List<int>() {1,6,11,16,21}},
                {13, new List<int>() {}},
                {14, new List<int>() {5,10,15,20,25}},
                {15, new List<int>() {}},
                {16, new List<int>() {}},
                {17, new List<int>() {}},
                {18, new List<int>() {21,22,23,24,25}},
                {19, new List<int>() {}},
                {20, new List<int>() {}},
                {21, new List<int>() {}},
                {22, new List<int>() {}},
                {23, new List<int>() {}},
                {24, new List<int>() {}},
                {25, new List<int>() {}}
            };
            this.outerNeighbours = new Dictionary<int, List<int>> ()
            {
                {1, new List<int>() {8,12}},
                {2, new List<int>() {8}},
                {3, new List<int>() {8}},
                {4, new List<int>() {8}},
                {5, new List<int>() {8,14}},
                {6, new List<int>() {12}},
                {7, new List<int>() {}},
                {8, new List<int>() {}},
                {9, new List<int>() {}},
                {10, new List<int>() {14}},
                {11, new List<int>() {12}},
                {12, new List<int>() {}},
                {13, new List<int>() {}},
                {14, new List<int>() {}},
                {15, new List<int>() {14}},
                {16, new List<int>() {12}},
                {17, new List<int>() {}},
                {18, new List<int>() {}},
                {19, new List<int>() {}},
                {20, new List<int>() {14}},
                {21, new List<int>() {12,18}},
                {22, new List<int>() {18}},
                {23, new List<int>() {18}},
                {24, new List<int>() {18}},
                {25, new List<int>() {14,18}}
            };
        }

        private List<List<string>> Iteration(List<List<string>> thisLayer, List<List<string>> outerLayer, List<List<string>> innerLayer)
        {
            var newLayer = GetEmptyLayer();  
            int nAdj;
            int xx = thisLayer[0].Count - 1;
            var yy = thisLayer.Count - 1;

            // Top left corner
            nAdj = (thisLayer[1][0] == "#" ? 1 : 0) + (thisLayer[0][1] == "#" ? 1 : 0);
            if (thisLayer[0][0] == "#")
            {
                newLayer[0][0] = nAdj == 1 ? "#" : "."; 
            }
            else
            {
                newLayer[0][0] = nAdj == 1 || nAdj == 2 ? "#" : "."; 
            }

            for (int y = 1; y < thisLayer.Count - 1; y++)
            {
                nAdj = (thisLayer[y-1][0] == "#" ? 1 : 0) + (thisLayer[y+1][0] == "#" ? 1 : 0) + (thisLayer[y][1] == "#" ? 1 : 0);
                if (thisLayer[y][0] == "#")
                {
                    newLayer[y][0] = nAdj == 1 ? "#" : "."; 
                }
                else
                {
                    newLayer[y][0] = nAdj == 1 || nAdj == 2 ? "#" : "."; 
                }
            }

            // Bottom left corner
            nAdj = (thisLayer[yy - 1][0] == "#" ? 1 : 0) + (thisLayer[yy][1] == "#" ? 1 : 0);
            if (thisLayer[yy][0] == "#")
            {
                newLayer[yy][0] = nAdj == 1 ? "#" : "."; 
            }
            else
            {
                newLayer[yy][0] = nAdj == 1 || nAdj == 2 ? "#" : "."; 
            }

            // Top right corner
            nAdj = (thisLayer[0][xx-1] == "#" ? 1 : 0) + (thisLayer[1][xx] == "#" ? 1 : 0);
            if (thisLayer[0][xx] == "#")
            {
                newLayer[0][xx] = nAdj == 1 ? "#" : "."; 
            }
            else
            {
                newLayer[0][xx] = nAdj == 1 || nAdj == 2 ? "#" : "."; 
            }

            for (int y = 1; y < thisLayer.Count - 1; y++)
            {
                nAdj = (thisLayer[y-1][xx] == "#" ? 1 : 0) + (thisLayer[y+1][xx] == "#" ? 1 : 0) + (thisLayer[y][xx-1] == "#" ? 1 : 0);
                if (thisLayer[y][xx] == "#")
                {
                    newLayer[y][xx] = nAdj == 1 ? "#" : "."; 
                }
                else
                {
                    newLayer[y][xx] = nAdj == 1 || nAdj == 2 ? "#" : "."; 
                }
            }

            // Bottom right corner
            nAdj = (thisLayer[yy - 1][xx] == "#" ? 1 : 0) + (thisLayer[yy][xx - 1] == "#" ? 1 : 0);
            if (thisLayer[yy][xx] == "#")
            {
                newLayer[yy][xx] = nAdj == 1 ? "#" : "."; 
            }
            else
            {
                newLayer[yy][xx] = nAdj == 1 || nAdj == 2 ? "#" : "."; 
            }

            // The rest.
            for (int x = 1; x < thisLayer[0].Count - 1; x++)
            {
                // Top row
                nAdj = (thisLayer[0][x - 1] == "#" ? 1 : 0) + (thisLayer[0][x + 1] == "#" ? 1 : 0) + (thisLayer[1][x] == "#" ? 1 : 0);
                if (thisLayer[0][x] == "#")
                {
                    newLayer[0][x] = nAdj == 1 ? "#" : "."; 
                }
                else
                {
                    newLayer[0][x] = nAdj == 1 || nAdj == 2 ? "#" : "."; 
                }
                // Middle rows
                for (int y = 1; y < thisLayer.Count - 1; y++)
                {
                    nAdj = (thisLayer[y][x - 1] == "#" ? 1 : 0) + (thisLayer[y][x + 1] == "#" ? 1 : 0) + (thisLayer[y - 1][x] == "#" ? 1 : 0) + (thisLayer[y + 1][x] == "#" ? 1 : 0);
                    if (thisLayer[y][x] == "#")
                    {
                        newLayer[y][x] = nAdj == 1 ? "#" : "."; 
                    }
                    else
                    {
                        newLayer[y][x] = nAdj == 1 || nAdj == 2 ? "#" : "."; 
                    }
                }

                // Bottom row
                nAdj = (thisLayer[yy][x - 1] == "#" ? 1 : 0) + (thisLayer[yy][x + 1] == "#" ? 1 : 0) + (thisLayer[yy - 1][x] == "#" ? 1 : 0);
                if (thisLayer[yy][x] == "#")
                {
                    newLayer[yy][x] = nAdj == 1 ? "#" : "."; 
                }
                else
                {
                    newLayer[yy][x] = nAdj == 1 || nAdj == 2 ? "#" : "."; 
                }
            }
            return newLayer;
        }

        private Dictionary<int, int> NewIteration(Dictionary<int, int> thisLayer, Dictionary<int, int> outerLayer, Dictionary<int, int> innerLayer)
        {
            var newLayer = GetEmptyIntLayer();  
            int nAdj;
            for (int i = 1; i < 26; i++)
            {
                nAdj = 0;
                foreach(var n in this.neighboursPartTwo[i])
                {
                    nAdj += thisLayer[n];
                }
                foreach(var n in this.outerNeighbours[i])
                {
                    nAdj += outerLayer[n];
                }
                foreach(var n in this.innerNeighbours[i])
                {
                    nAdj += innerLayer[n];
                }

                if (thisLayer[i] == 1)
                {
                    newLayer[i] = nAdj == 1 ? 1 : 0; 
                }
                else
                {
                    newLayer[i] = nAdj == 1 || nAdj == 2 ? 1 : 0; 
                }
            }
            return newLayer;
        }
        private List<List<string>> GetEmptyLayer()
        {
            return new List<List<string>>() { 
                new List<string>() {".", ".", ".", ".", "."},
                new List<string>() {".", ".", ".", ".", "."},
                new List<string>() {".", ".", ".", ".", "."},
                new List<string>() {".", ".", ".", ".", "."},
                new List<string>() {".", ".", ".", ".", "."}, 
            };  
        }

        private Dictionary<int, int> GetEmptyIntLayer()
        {
            var x = new Dictionary<int, int>(Enumerable.Range(1,25).Select(x => new KeyValuePair<int, int>(x, 0)));
            return x;
        }

        private int Biodiversity(List<List<string>> map)
        {
            return map.SelectMany(i => i).Select((x,i) => x == "#" ? (int)Math.Round(Math.Pow(2,i)) : 0).Sum();
        }

        public string SolvePartOne()
        {
            var map = this.map;
            var seen = new HashSet<string>();
            while (true)
            {
                map = Iteration(map, GetEmptyLayer(), GetEmptyLayer());
                var thismap = String.Join("", map.SelectMany(i => i));
                if (seen.Contains(thismap))
                {
                    return Biodiversity(map).ToString();
                }
                seen.Add(thismap);
            }
        }
    
        public string SolvePartTwo()
        {
            var maps = new List<Dictionary<int, int>>();
            maps.Add(this.GetEmptyIntLayer());
            maps.Add(this.intmap);
            maps.Add(this.GetEmptyIntLayer());

            int n = 0;
            while (n < 200)
            {
                var newMaps = new List<Dictionary<int, int>>();
                
                newMaps.Add(NewIteration(GetEmptyIntLayer(), GetEmptyIntLayer(), maps[0]));
                newMaps.Add(NewIteration(maps[0], GetEmptyIntLayer(), maps[1]));
                for (int i = 1; i < maps.Count - 1; i++)
                {
                    newMaps.Add(NewIteration(maps[i], maps[i-1], maps[i+1]));
                }
                newMaps.Add(NewIteration(maps.Last(), maps[maps.Count - 2], this.GetEmptyIntLayer()));
                newMaps.Add(NewIteration(GetEmptyIntLayer(), maps.Last(), this.GetEmptyIntLayer()));
                while (newMaps.First().Values.Sum() == 0)
                {
                    newMaps.RemoveAt(0);
                }
                while (newMaps.Last().Values.Sum() == 0)
                {
                    newMaps.RemoveAt(newMaps.Count - 1);
                }
                maps = newMaps;
                n += 1;
            }
            return maps.Select(map => map.Values.Sum()).Sum().ToString();
        }
    }
}
