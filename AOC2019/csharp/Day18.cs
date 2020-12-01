using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day18 : IDay
    {
        private readonly string data;
        private Dictionary<(int x, int y), string> map;
        private readonly IEnumerable<KeyValuePair<(int x, int y), string>> keys;
        private readonly int nKeys;
        private List<long> values;

        public Day18()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input18.txt"));
            //this.data = "########################\n#@..............ac.GI.b#\n###d#e#f################\n###A#B#C################\n###g#h#i################\n########################";
            //this.data ="#################\n#i.G..c...e..H.p#\n########.########\n#j.A..b...f..D.o#\n########@########\n#k.E..a...g..B.n#\n########.########\n#l.F..d...h..C.m#\n#################";
            this.map = ParseMap();
            this.keys = this.map.Distinct().Where(o => o.Value[0] >= 97 && o.Value[0] <= 122).ToList();
            this.nKeys = this.map.Distinct().Where(o => o.Value[0] >= 97 && o.Value[0] <= 122).Count();
        }

        public string SolvePartOne()
        {
            var x = this.map.Where(o => o.Value == "@").First().Key;
            Dictionary<(string start, string end), (int steps, HashSet<string> requiredKeys)> keyDistances = 
                new Dictionary<(string start, string end), (int steps, HashSet<string> requiredKeys)>();
            var allItems = (new List<string>() {"@"}).Concat(this.keys.Select(o => o.Value)).ToList();
            foreach (var fromKey in allItems)
            {
                var y = this.keys.Where(o => o.Value == fromKey);
                (int x, int y) start = y.Count() == 0 ? x : y.First().Key;
                var distAndReqs = FindToFromDistanceAndRequirements(start);
                foreach (var toKey in distAndReqs)
                {
                    if (keyDistances.ContainsKey((fromKey, toKey.key)))
                    {
                        if (toKey.steps < keyDistances[(fromKey, toKey.key)].steps)
                        {
                            keyDistances[(fromKey, toKey.key)] = (toKey.steps, toKey.requiredKeys);
                        }
                    }
                    else
                    {
                        keyDistances[(fromKey, toKey.key)] = (toKey.steps, toKey.requiredKeys);
                    }
                }
            }
            var distanceCombos = new List<(List<string> keysPicked, int distance)>();
            // Add all first distances.
            foreach (var startToKey in keyDistances.Where(keyDistances => keyDistances.Key.start == "@").Where(k=> k.Value.requiredKeys.Count == 0))
            {
                distanceCombos.Add((new List<string>() {startToKey.Key.end, }, startToKey.Value.steps));
            }

            // Now find all combinations...
            var paths = new List<int>();
            while (distanceCombos.Count > 0)
            {
                var combo = distanceCombos[0];
                distanceCombos.RemoveAt(0);
                if (combo.keysPicked.Count == this.nKeys)
                {
                    paths.Add(combo.distance);
                }
                foreach (var nextKey in allItems.Where(o => o != "@" && !combo.keysPicked.Contains(o)))
                {
                    //var thisPosition = this.keys.Where(o => o.Value == nextKey).First().Key;
                    var movement = keyDistances[(combo.keysPicked.Last(), nextKey)];
                    bool hasRequiredKeys = movement.requiredKeys.Intersect(combo.keysPicked).Count() == movement.requiredKeys.Count;
                    if (hasRequiredKeys)
                    {
                        var keys = new List<string>(combo.keysPicked);
                        keys.Add(nextKey);
                        distanceCombos.Add((keys, combo.distance + movement.steps));
                        if (distanceCombos
                            //.Where(o => o.distance <= (combo.distance + movement.steps))
                            //.Where(o => keys.Intersect(o.keysPicked).Count() == keys.Count)
                            .Where(o => o.keysPicked.SequenceEqual(keys))
                            .Count() == 0)
                        {
                            distanceCombos.Add((keys, combo.distance + movement.steps));
                            //Console.WriteLine($"Took {nextKey} {combo.distance + movement.steps} | {String.Join(", ", keys)}");
                        }
                        //Console.WriteLine($"Took {nextKey} {combo.distance + movement.steps} | {String.Join(", ", keys)}");
                    }
                }
                distanceCombos.Sort((a,b) => a.distance.CompareTo(b.distance));
            }
            return paths.Min().ToString();
        }
        
        public string SolvePartTwo()
        {
            return "";
        }
        
        private Dictionary<(int x, int y), string> ParseMap()
        {
            Dictionary<(int x, int y), string> map = new Dictionary<(int x, int y), string>();
            int row = 0;
            foreach (var rowstr in this.data.Trim().Split('\n'))
            {
                int col = 0;
                foreach(var cell in rowstr)
                {
                    map[(col, row)] = cell.ToString();
                    col += 1;
                }
                row += 1;
            }
            return map;
        }

        private List<(string key, int steps, HashSet<string> requiredKeys)> FindToFromDistanceAndRequirements((int x, int y) startingPosition)
        {
            var moves = new List<(int x, int y)>() {(0,1), (0,-1), (1,0), (-1,0)};
            var states = new List<State>();
            var startingVisited = new HashSet<(int x, int y)>();
            var required = new HashSet<string>();
            startingVisited.Add(startingPosition);
            states.Add(new State(startingPosition, 0, startingVisited, new HashSet<string>()));
            var output = new List<(string key, int steps, HashSet<string> requiredKeys)>();
            while (states.Count > 0)
            {
                var state = states.First();
                states.RemoveAt(0);
                foreach (var move in moves)
                {
                    var newPos = (x: state.Position.x + move.x, y: state.Position.y + move.y);
                    var thisCell = this.map[newPos];
                    if (thisCell == "#" || state.Visited.Contains(newPos))
                    {
                        continue;
                    }
                    else if (thisCell[0] >= 97 && thisCell[0] <= 122)
                    {
                        output.Add((thisCell, state.NSteps + 1, state.RequiredKeys));
                        var visited = new HashSet<(int x, int y)>(state.Visited);
                        visited.Add(newPos);
                        states.Add(new State(newPos, state.NSteps + 1, visited, state.RequiredKeys));
                    }
                    else if (thisCell[0] >= 65 && thisCell[0] <= 90)
                    {
                        var requiredKeys = new HashSet<string>(state.RequiredKeys);
                        requiredKeys.Add(thisCell.ToLower());
                        var visited = new HashSet<(int x, int y)>(state.Visited);
                        visited.Add(newPos);
                        states.Add(new State(newPos, state.NSteps + 1, visited, requiredKeys));
                    }
                    else
                    {
                        var visited = new HashSet<(int x, int y)>(state.Visited);
                        visited.Add(newPos);
                        states.Add(new State(newPos, state.NSteps + 1, visited, state.RequiredKeys));
                    }
                }
                states.Sort((a,b) => a.NSteps.CompareTo(b.NSteps));
            }
            return output;
        }

     public class State
     {
         public HashSet<string> RequiredKeys;
         public (int x, int y) Position { get; }
         public (int x, int y) PreviousPosition { get; }
         public int NSteps { get; }
         public HashSet<(int x, int y)> Visited { get; }

         public State((int x, int y) position, int nSteps, HashSet<(int x, int y)> visited, HashSet<string> requiredKeys)
         {
             Position = position;
             Visited = visited;
             RequiredKeys = requiredKeys;
             NSteps = nSteps;
         }
     }
     }
}
