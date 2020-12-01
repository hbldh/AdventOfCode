using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day20 : IDay
    {
        private readonly string data;
        private Dictionary<(int x, int y), List<(int x, int y)>> map;
        private Dictionary<(int x, int y), string> cell2portalName;
        private Dictionary<string, List<(int x, int y)>> portalName2cells;

        private (int x, int y) size;

        public Day20()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input20.txt"));
            //this.data = "         A           \n         A           \n  #######.#########  \n  #######.........#  \n  #######.#######.#  \n  #######.#######.#  \n  #######.#######.#  \n  #####  B    ###.#  \nBC...##  C    ###.#  \n  ##.##       ###.#  \n  ##...DE  F  ###.#  \n  #####    G  ###.#  \n  #########.#####.#  \nDE..#######...###.#  \n  #.#########.###.#  \nFG..#########.....#  \n  ###########.#####  \n             Z       \n             Z       ";
            //this.data ="                   A               \n                   A               \n  #################.#############  \n  #.#...#...................#.#.#  \n  #.#.#.###.###.###.#########.#.#  \n  #.#.#.......#...#.....#.#.#...#  \n  #.#########.###.#####.#.#.###.#  \n  #.............#.#.....#.......#  \n  ###.###########.###.#####.#.#.#  \n  #.....#        A   C    #.#.#.#  \n  #######        S   P    #####.#  \n  #.#...#                 #......VT\n  #.#.#.#                 #.#####  \n  #...#.#               YN....#.#  \n  #.###.#                 #####.#  \nDI....#.#                 #.....#  \n  #####.#                 #.###.#  \nZZ......#               QG....#..AS\n  ###.###                 #######  \nJO..#.#.#                 #.....#  \n  #.#.#.#                 ###.#.#  \n  #...#..DI             BU....#..LF\n  #####.#                 #.#####  \nYN......#               VT..#....QG\n  #.###.#                 #.###.#  \n  #.#...#                 #.....#  \n  ###.###    J L     J    #.#.###  \n  #.....#    O F     P    #.#...#  \n  #.###.#####.#.#####.#####.###.#  \n  #...#.#.#...#.....#.....#.#...#  \n  #.#####.###.###.#.#.#########.#  \n  #...#.#.....#...#.#.#.#.....#.#  \n  #.###.#####.###.###.#.#.#######  \n  #.#.........#...#.............#  \n  #########.###.###.#############  \n           B   J   C               \n           U   P   P               ";
            //this.data = "             Z L X W       C                 \n             Z P Q B       K                 \n  ###########.#.#.#.#######.###############  \n  #...#.......#.#.......#.#.......#.#.#...#  \n  ###.#.#.#.#.#.#.#.###.#.#.#######.#.#.###  \n  #.#...#.#.#...#.#.#...#...#...#.#.......#  \n  #.###.#######.###.###.#.###.###.#.#######  \n  #...#.......#.#...#...#.............#...#  \n  #.#########.#######.#.#######.#######.###  \n  #...#.#    F       R I       Z    #.#.#.#  \n  #.###.#    D       E C       H    #.#.#.#  \n  #.#...#                           #...#.#  \n  #.###.#                           #.###.#  \n  #.#....OA                       WB..#.#..ZH\n  #.###.#                           #.#.#.#  \nCJ......#                           #.....#  \n  #######                           #######  \n  #.#....CK                         #......IC\n  #.###.#                           #.###.#  \n  #.....#                           #...#.#  \n  ###.###                           #.#.#.#  \nXF....#.#                         RF..#.#.#  \n  #####.#                           #######  \n  #......CJ                       NM..#...#  \n  ###.#.#                           #.###.#  \nRE....#.#                           #......RF\n  ###.###        X   X       L      #.#.#.#  \n  #.....#        F   Q       P      #.#.#.#  \n  ###.###########.###.#######.#########.###  \n  #.....#...#.....#.......#...#.....#.#...#  \n  #####.#.###.#######.#######.###.###.#.#.#  \n  #.......#.......#.#.#.#.#...#...#...#.#.#  \n  #####.###.#####.#.#.#.#.###.###.#.###.###  \n  #.......#.....#.#...#...............#...#  \n  #############.#.#.###.###################  \n               A O F   N                     \n               A A D   M                     ";
            ParseMap();
        }

        public string SolvePartOne()
        {
            var moves = new List<(int x, int y)>() {(0,1), (0,-1), (1,0), (-1,0)};
            var states = new List<List<(int x, int y)>>() {new List<(int x, int y)>() {this.portalName2cells["AA"].First(),}};
            var zzPos = this.portalName2cells["ZZ"].First();
            var paths = new List<List<(int x, int y)>>();
            while (states.Count > 0)
            {
                var currentState = states[0];
                states.RemoveAt(0);
                var currentPosition = currentState.Last();
                if (currentPosition.Equals(zzPos))
                {
                    return (currentState.Where(x => !this.cell2portalName.ContainsKey(x)).Count() - 1).ToString();
                }
                foreach(var move in moves)
                {
                    if (this.map.ContainsKey((currentPosition.x + move.x, currentPosition.y+move.y)))
                    {
                        // Available move.
                        var newPosition = (currentPosition.x + move.x, currentPosition.y + move.y);
                        if (currentState.Contains(newPosition)) continue;
                        var newstate = new List<(int x, int y)>(currentState);
                        newstate.Add(newPosition);
                        states.Add(newstate);
                    }
                    else if (this.cell2portalName.ContainsKey((currentPosition.x + move.x, currentPosition.y+move.y))) 
                    {
                        var newPosition = (currentPosition.x + move.x, currentPosition.y + move.y);
                        var portalName = this.cell2portalName[(currentPosition.x + move.x, currentPosition.y+move.y)];
                        var newstate = new List<(int x, int y)>(currentState);
                        newstate.Add(newPosition);
                        if (portalName != "AA" && portalName != "ZZ")
                        {
                            var portalLocation = this.portalName2cells[portalName].Where(x => !x.Equals(newPosition)).First();
                            newstate.Add(portalLocation);
                        }
                        states.Add(newstate);
                    }
                }
                states.Sort((a,b) => a.Where(x => !this.cell2portalName.ContainsKey(x)).Count().CompareTo(b.Where(x => !this.cell2portalName.ContainsKey(x)).Count()));
            }
            return "";
        }

        private List<(string portal, (int x, int y) position, bool isInnerPortal, int distance)> FindReachablePortals((int x, int y) position)
        {
            var reachablePortals = new List<(string portal, (int x, int y) position, bool isInnerPortal, int distance)>();
            var moves = new List<(int x, int y)>() {(0,1), (0,-1), (1,0), (-1,0)};
            var states = new List<List<(int x, int y)>>() {new List<(int x, int y)>() {position,}};
            while (states.Count > 0)
            {
                var currentState = states[0];
                states.RemoveAt(0);
                var currentPosition = currentState.Last();
                foreach(var move in moves)
                {
                    if (this.map.ContainsKey((currentPosition.x + move.x, currentPosition.y+move.y)))
                    {
                        // Available move.
                        var newPosition = (currentPosition.x + move.x, currentPosition.y + move.y);
                        if (currentState.Contains(newPosition)) continue;
                        var newstate = new List<(int x, int y)>(currentState);
                        newstate.Add(newPosition);
                        states.Add(newstate);
                    }
                    else if (this.cell2portalName.ContainsKey((currentPosition.x + move.x, currentPosition.y+move.y))) 
                    {
                        var newPosition = (x: currentPosition.x + move.x, y: currentPosition.y + move.y);
                        if (currentState.Contains(newPosition)) continue;
                        var portalName = this.cell2portalName[(currentPosition.x + move.x, currentPosition.y+move.y)];
                        var newstate = new List<(int x, int y)>(currentState);
                        newstate.Add(newPosition);
                        if (portalName != "AA" && portalName != "ZZ")
                        {
                            bool isInnerPortal = !(newPosition.x == 1 || newPosition.x == (this.size.x - 2) || newPosition.y == 1 || newPosition.y == (this.size.y - 2));
                            var portalLocation = this.portalName2cells[portalName].Where(x => x.x != newPosition.x && x.y != newPosition.y).First();
                            reachablePortals.Add((portalName, portalLocation, isInnerPortal, currentState.Where(x => !this.cell2portalName.ContainsKey(x)).Count() - 1));
                        }
                        else if (portalName == "ZZ")
                        {
                            var portalLocation = this.portalName2cells[portalName].First();
                            reachablePortals.Add((portalName, portalLocation, false, currentState.Where(x => !this.cell2portalName.ContainsKey(x)).Count() - 1));
                        }
                    }
                }
                states.Sort((a,b) => a.Where(x => !this.cell2portalName.ContainsKey(x)).Count().CompareTo(b.Where(x => !this.cell2portalName.ContainsKey(x)).Count()));
            }
            return reachablePortals;
        }
        
        public string SolvePartTwo()
        {
            var portalMap = new Dictionary<(string portal, (int x, int y) position), List<(string portal, (int x, int y) position, bool isInnerPortal, int distance)>>();
            foreach (var portal in this.portalName2cells)
            {
                foreach(var portalPoint in portal.Value)
                {
                    var reachablePortals = FindReachablePortals(portalPoint);
                    portalMap[(portal.Key, portalPoint)] = reachablePortals;
                }
            }

            var zzPos = (x: this.portalName2cells["ZZ"].First().x, y: this.portalName2cells["ZZ"].First().y, level:0);
            var states = new List<(List<(int x, int y)> positions, int distance, int level)>() 
            {
                (new List<(int x, int y)>() {this.portalName2cells["AA"].First(), }, 0, 0)
            };
            while (states.Count > 0)
            {
                var currentState = states[0];
                var currentPosition = currentState.positions.Last();
                states.RemoveAt(0);
                if (currentPosition.x == zzPos.x && currentPosition.y == zzPos.y)
                {
                    return (currentState.distance - 1).ToString();
                }
                var whereAreWe = this.cell2portalName[currentPosition];
                foreach (var whereWeCanGo in portalMap[(whereAreWe, currentPosition)])
                {
                    int nextLevel = whereWeCanGo.isInnerPortal ? currentState.level + 1 : currentState.level - 1;
                    if (whereWeCanGo.portal == "ZZ")
                    {
                        if (currentState.level == 0)
                        {
                            nextLevel = 0;
                        }
                        else
                        {
                            continue;
                        }
                        
                    }
                    else if (nextLevel < 0)
                    {
                        continue;
                    }
                    var backtrack = new List<(int x, int y)>(currentState.positions);
                    backtrack.Add(whereWeCanGo.position);
                    var newState = (backtrack, currentState.distance + whereWeCanGo.distance + 1, nextLevel);
                    states.Add(newState);
                }
                states.Sort((a,b) => a.distance.CompareTo(b.distance));
            }
            return "";
        }
        public string SolvePartTwo_NotUsed()
        {
            var moves = new List<(int x, int y)>() {(0,1), (0,-1), (1,0), (-1,0)};
            var states = new List<List<(int x, int y, int level)>>() {new List<(int x, int y, int level)>() 
                {(this.portalName2cells["AA"].First().x,this.portalName2cells["AA"].First().y, 0)}
            };
            var zzPos = (x: this.portalName2cells["ZZ"].First().x, y: this.portalName2cells["ZZ"].First().y, level:0);
            while (states.Count > 0)
            {
                var currentState = states[0];
                states.RemoveAt(0);
                var currentPosition = currentState.Last();
                if (currentPosition.x == zzPos.x && currentPosition.y == zzPos.y && currentPosition.level == zzPos.level)
                {
                    return (currentState.Where(x => !this.cell2portalName.ContainsKey((x.x, x.y))).Count() - 1).ToString();
                }
                foreach(var move in moves)
                {
                    if (this.map.ContainsKey((currentPosition.x + move.x, currentPosition.y+move.y)))
                    {
                        // Available move.
                        var newPosition = (x: currentPosition.x + move.x, y: currentPosition.y + move.y, level: currentPosition.level);
                        if (currentState.Where(x=> x.x == newPosition.x && x.y == newPosition.y && x.level == newPosition.level).Count() > 0) continue;
                        var newstate = new List<(int x, int y, int level)>(currentState);
                        newstate.Add(newPosition);
                        states.Add(newstate);
                    }
                    else if (this.cell2portalName.ContainsKey((currentPosition.x + move.x, currentPosition.y+move.y))) 
                    {
                        var newPosition = (x: currentPosition.x + move.x, y: currentPosition.y + move.y, level: currentPosition.level);
                        if (currentState.Where(x=> x.x == newPosition.x && x.y == newPosition.y && x.level == newPosition.level).Count() > 0) continue;
                        var portalName = this.cell2portalName[(currentPosition.x + move.x, currentPosition.y+move.y)];
                        var newstate = new List<(int x, int y, int level)>(currentState);
                        
                        if (portalName != "AA" && portalName != "ZZ")
                        {
                            newstate.Add(newPosition);
                            var portalLocation = this.portalName2cells[portalName].Where(x => x.x != newPosition.x && x.y != newPosition.y).First();
                            if (newPosition.x == 1 || newPosition.x == (this.size.x - 1) || newPosition.y == 1 || newPosition.y == (this.size.y - 1))
                            {
                                // Outer portal: move up one level.
                                if (currentPosition.level != 0) // Only outer portals on level 1+
                                {
                                    newstate.Add((portalLocation.x, portalLocation.y, currentPosition.level - 1));
                                }
                            }
                            else
                            {
                                // Inner portal: Move down one level.
                                newstate.Add((portalLocation.x, portalLocation.y, currentPosition.level + 1));
                            }
                            states.Add(newstate);
                        }
                        else
                        {
                            if (currentPosition.level == 0)
                            {
                                newstate.Add(newPosition);
                            }
                            else
                            {
                                // Just a wall no lower levels...
                            }
                        }
                    }
                }
                states.Sort((a,b) => a.Where(x => !this.cell2portalName.ContainsKey((x.x, x.y))).Count().CompareTo(b.Where(x => !this.cell2portalName.ContainsKey((x.x, x.y))).Count()));
            }
            return "";
        }

        private void ParseMap()
        {
            var moves = new List<(int x, int y)>() {(0,1), (0,-1), (1,0), (-1,0)};
            
            this.map = new Dictionary<(int x, int y), List<(int x, int y)>>();
            this.cell2portalName = new Dictionary<(int x, int y), string>();
            this.portalName2cells = new Dictionary<string, List<(int x, int y)>>();

            var cells = this.data.Trim('\n').Split('\n').Select(x => x.Trim('\n').ToCharArray().Select(v => v.ToString()).ToList()).ToList();
            this.size = (cells[0].Count, cells.Count);

            // Find all pathways (nodes) and their connections
            for (int x = 2; x < cells[0].Count - 2; x++)
            {
                for (int y = 2; y < cells.Count - 2; y++)
                {
                    if (cells[y][x] == ".")
                    {
                        var edges = new List<(int x, int y)>();
                        foreach (var move in moves)
                        {
                            var cell = cells[y + move.y][x + move.x];
                            if (cell == ".")
                            {
                                edges.Add((x + move.x, y + move.y));
                            }
                            else if ((cell[0] >= 65 && cell[0] <= 90))
                            {
                                // Portal to this side.
                                edges.Add((x + move.x, y + move.y));
                                string portalName;
                                if (move.x < 0 || move.y < 0)
                                {
                                    portalName = cells[y + move.y*2][x + move.x*2] + cell;
                                }
                                else
                                {
                                    portalName = cell + cells[y + move.y*2][x + move.x*2];
                                }
                                cell2portalName[(x + move.x, y + move.y)] = portalName;
                                portalName2cells[portalName] = new List<(int x, int y)>();
                            }
                        }
                        this.map[(x,y)] = edges;
                    }
                }
            }
            // Reverse the cell2portalName dict and aggregate.
            foreach (var portal in cell2portalName)
            {
                this.portalName2cells[portal.Value].Add(portal.Key);
            }
            // Block all deadends.
            while (true)
            {
                var deadends = this.map.Where(kv => kv.Value.Count < 2).ToList();
                if (deadends.Count == 0)
                {
                    break;
                }
                foreach(var cellAndEdge in deadends)
                {
                    this.map.Remove(cellAndEdge.Key);
                    foreach (var connectedNode in this.map.Where(kv => kv.Value.Contains(cellAndEdge.Key)).ToList())
                    {
                        connectedNode.Value.Remove(cellAndEdge.Key);
                        this.map[connectedNode.Key] = connectedNode.Value;
                    }
                }
            }
            //PrintMap();
        }
        private void PrintMap()
        {
            var cells = this.data.Trim('\n').Split('\n').Select(x => x.ToCharArray().Select(v => v.ToString()).ToList()).ToList();

            for (int y = 1; y < cells.Count - 1; y++)
            {
                for (int x = 1; x < cells[0].Count - 1; x++)
                {
                    if (this.map.ContainsKey((x,y)))
                    {
                        Console.Write(".");
                    }  
                    else
                    {
                        if (this.cell2portalName.ContainsKey((x,y)))
                        {
                            Console.Write("P");
                        }
                        else {
                            Console.Write("#");
                        }
                        
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
