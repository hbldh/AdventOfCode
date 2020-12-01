using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AOC2019
{
    class Day12 : IDay
    {
        private readonly string data;
        private List<((string name, long qty) output, List<(string name, long qty)> input)> elements;

        public List<string> moonNames { get; private set; }

        public Day12()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            //this.data = "<x=-1, y=0, z=2>\n<x=2, y=-10, z=-7>\n<x=4, y=-8, z=8>\n<x=3, y=5, z=-1>";
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input12.txt"));
            this.moonNames = new List<string>() {"Io", "Europa", "Ganymede", "Callisto"};
        }

        private Dictionary<string, List<(int iteration, int x, int y, int z, int vx, int vy, int vz)>> PerformIterations(int n)
        {
            var iterationHashcodes = new Dictionary<int, int>();
            var startingPositions = this.moonNames.Zip(this.data.Trim().Split('\n'), (name, posStr)=> (
                name: name, 
                x: int.Parse((new Regex("x=(-?[0-9]+)")).Match(posStr).Groups[1].Value), 
                y: int.Parse((new Regex("y=(-?[0-9]+)")).Match(posStr).Groups[1].Value),
                z: int.Parse((new Regex("z=(-?[0-9]+)")).Match(posStr).Groups[1].Value),
                v: 0
                )
            ).ToList();
            
            var iterations2State = new Dictionary<string, List<(int iteration, int x, int y, int z, int vx, int vy, int vz)>>();
            iterations2State[startingPositions[0].name] = new List<(int iteration, int x, int y, int z, int vx, int vy, int vz)>() {(0, startingPositions[0].x, startingPositions[0].y, startingPositions[0].z, 0,0,0)};
            iterations2State[startingPositions[1].name] = new List<(int iteration, int x, int y, int z, int vx, int vy, int vz)>() {(0, startingPositions[1].x, startingPositions[1].y, startingPositions[1].z, 0,0,0)};
            iterations2State[startingPositions[2].name] = new List<(int iteration, int x, int y, int z, int vx, int vy, int vz)>() {(0, startingPositions[2].x, startingPositions[2].y, startingPositions[2].z, 0,0,0)};
            iterations2State[startingPositions[3].name] = new List<(int iteration, int x, int y, int z, int vx, int vy, int vz)>() {(0, startingPositions[3].x, startingPositions[3].y, startingPositions[3].z, 0,0,0)};

            for (int i = 1; i <= n; i++)
            {
                var newStates = new List<(int iteration, int x, int y, int z, int vx, int vy, int vz)>() {
                    iterations2State[moonNames[0]].Last(),
                    iterations2State[moonNames[1]].Last(),
                    iterations2State[moonNames[2]].Last(),
                    iterations2State[moonNames[3]].Last(),
                };
                var newVelocities = new List<List<int>>() {
                    new List<int> {newStates[0].vx, newStates[0].vy, newStates[0].vz},
                    new List<int> {newStates[1].vx, newStates[1].vy, newStates[1].vz},
                    new List<int> {newStates[2].vx, newStates[2].vy, newStates[2].vz},
                    new List<int> {newStates[3].vx, newStates[3].vy, newStates[3].vz},
                };

                for (int moonA = 0; moonA < moonNames.Count; moonA++)
                {
                    for (int moonB = moonA + 1; moonB < moonNames.Count; moonB++)
                    {
                        var lastState = iterations2State[moonNames[moonA]].Last();
                        var lastStateOther = iterations2State[moonNames[moonB]].Last();
                        // X
                        if (lastState.x > lastStateOther.x && lastStateOther.x < lastState.x)
                        {
                            newVelocities[moonA][0] -= 1;
                            newVelocities[moonB][0] += 1;
                        }
                        else if (lastStateOther.x > lastState.x && lastState.x < lastStateOther.x)
                        {
                            newVelocities[moonA][0] += 1;
                            newVelocities[moonB][0] -= 1;
                        }
                        // Y
                        if (lastState.y > lastStateOther.y && lastStateOther.y < lastState.y)
                        {
                            newVelocities[moonA][1] -= 1;
                            newVelocities[moonB][1] += 1;
                        }
                        else if (lastStateOther.y > lastState.y && lastState.y < lastStateOther.y)
                        {
                            newVelocities[moonA][1] += 1;
                            newVelocities[moonB][1] -= 1;
                        }
                        // Z
                        if (lastState.z > lastStateOther.z && lastStateOther.z < lastState.z)
                        {
                            newVelocities[moonA][2] -= 1;
                            newVelocities[moonB][2] += 1;
                        }
                        else if (lastStateOther.z > lastState.z && lastState.z < lastStateOther.z)
                        {
                            newVelocities[moonA][2] += 1;
                            newVelocities[moonB][2] -= 1;
                        }
                    }
                }

                
                for (int moon = 0; moon < moonNames.Count; moon++)
                {
                    var newState = newStates[moon];
                    iterations2State[moonNames[moon]].Add(
                        (
                            i, 
                            newState.x + newVelocities[moon][0], 
                            newState.y + newVelocities[moon][1], 
                            newState.z + newVelocities[moon][2], 
                            newVelocities[moon][0], 
                            newVelocities[moon][1], 
                            newVelocities[moon][2]
                        )
                    );
                }
                bool matchesInitial = true;
                for (int moon = 0; moon < moonNames.Count; moon++)
                {
                    matchesInitial &= iterations2State[moonNames[moon]].Last().x == iterations2State[moonNames[moon]].First().x;
                    matchesInitial &= iterations2State[moonNames[moon]].Last().y == iterations2State[moonNames[moon]].First().y;
                    matchesInitial &= iterations2State[moonNames[moon]].Last().z == iterations2State[moonNames[moon]].First().z;
                }
                if (matchesInitial)
                {
                    return iterations2State;
                }
            }
            return iterations2State;
        }

         private List<int> FindCycle(int n)
        {
            var startingPositions = this.moonNames.Zip(this.data.Trim().Split('\n'), (name, posStr)=> (
                name: name, 
                x: int.Parse((new Regex("x=(-?[0-9]+)")).Match(posStr).Groups[1].Value), 
                y: int.Parse((new Regex("y=(-?[0-9]+)")).Match(posStr).Groups[1].Value),
                z: int.Parse((new Regex("z=(-?[0-9]+)")).Match(posStr).Groups[1].Value),
                v: 0
                )
            ).ToList();
            var matchesInitial = new List<int>() {0,0,0};
            
            var iterations2State = new Dictionary<string, List<(int iteration, int x, int y, int z, int vx, int vy, int vz)>>();
            iterations2State[startingPositions[0].name] = new List<(int iteration, int x, int y, int z, int vx, int vy, int vz)>() {(0, startingPositions[0].x, startingPositions[0].y, startingPositions[0].z, 0,0,0)};
            iterations2State[startingPositions[1].name] = new List<(int iteration, int x, int y, int z, int vx, int vy, int vz)>() {(0, startingPositions[1].x, startingPositions[1].y, startingPositions[1].z, 0,0,0)};
            iterations2State[startingPositions[2].name] = new List<(int iteration, int x, int y, int z, int vx, int vy, int vz)>() {(0, startingPositions[2].x, startingPositions[2].y, startingPositions[2].z, 0,0,0)};
            iterations2State[startingPositions[3].name] = new List<(int iteration, int x, int y, int z, int vx, int vy, int vz)>() {(0, startingPositions[3].x, startingPositions[3].y, startingPositions[3].z, 0,0,0)};

            for (int i = 1; i <= n; i++)
            {
                var newStates = new List<(int iteration, int x, int y, int z, int vx, int vy, int vz)>() {
                    iterations2State[moonNames[0]].Last(),
                    iterations2State[moonNames[1]].Last(),
                    iterations2State[moonNames[2]].Last(),
                    iterations2State[moonNames[3]].Last(),
                };
                var newVelocities = new List<List<int>>() {
                    new List<int> {newStates[0].vx, newStates[0].vy, newStates[0].vz},
                    new List<int> {newStates[1].vx, newStates[1].vy, newStates[1].vz},
                    new List<int> {newStates[2].vx, newStates[2].vy, newStates[2].vz},
                    new List<int> {newStates[3].vx, newStates[3].vy, newStates[3].vz},
                };

                for (int moonA = 0; moonA < moonNames.Count; moonA++)
                {
                    for (int moonB = moonA + 1; moonB < moonNames.Count; moonB++)
                    {
                        var lastState = iterations2State[moonNames[moonA]].Last();
                        var lastStateOther = iterations2State[moonNames[moonB]].Last();
                        // X
                        if (lastState.x > lastStateOther.x && lastStateOther.x < lastState.x)
                        {
                            newVelocities[moonA][0] -= 1;
                            newVelocities[moonB][0] += 1;
                        }
                        else if (lastStateOther.x > lastState.x && lastState.x < lastStateOther.x)
                        {
                            newVelocities[moonA][0] += 1;
                            newVelocities[moonB][0] -= 1;
                        }
                        // Y
                        if (lastState.y > lastStateOther.y && lastStateOther.y < lastState.y)
                        {
                            newVelocities[moonA][1] -= 1;
                            newVelocities[moonB][1] += 1;
                        }
                        else if (lastStateOther.y > lastState.y && lastState.y < lastStateOther.y)
                        {
                            newVelocities[moonA][1] += 1;
                            newVelocities[moonB][1] -= 1;
                        }
                        // Z
                        if (lastState.z > lastStateOther.z && lastStateOther.z < lastState.z)
                        {
                            newVelocities[moonA][2] -= 1;
                            newVelocities[moonB][2] += 1;
                        }
                        else if (lastStateOther.z > lastState.z && lastState.z < lastStateOther.z)
                        {
                            newVelocities[moonA][2] += 1;
                            newVelocities[moonB][2] -= 1;
                        }
                    }
                }
                
                for (int moon = 0; moon < moonNames.Count; moon++)
                {
                    var newState = newStates[moon];
                    iterations2State[moonNames[moon]].Add(
                        (
                            i, 
                            newState.x + newVelocities[moon][0], 
                            newState.y + newVelocities[moon][1], 
                            newState.z + newVelocities[moon][2], 
                            newVelocities[moon][0], 
                            newVelocities[moon][1], 
                            newVelocities[moon][2]
                        )
                    );
                }

                bool xMatch = true;
                bool yMatch = true;
                bool zMatch = true;
                for (int moon = 0; moon < moonNames.Count; moon++)
                {
                    xMatch &= (iterations2State[moonNames[moon]].Last().x == iterations2State[moonNames[moon]].First().x);
                    yMatch &= (iterations2State[moonNames[moon]].Last().y == iterations2State[moonNames[moon]].First().y);
                    zMatch &= (iterations2State[moonNames[moon]].Last().z == iterations2State[moonNames[moon]].First().z);
                }
                if (xMatch && matchesInitial[0] == 0)
                {
                    matchesInitial[0] = i + 1;
                }
                if (yMatch&& matchesInitial[1] == 0)
                {
                    matchesInitial[1] = i + 1;
                }
                if (zMatch&& matchesInitial[2] == 0)
                {
                    matchesInitial[2] = i + 1;
                }
                if (matchesInitial.All(x => x > 0))
                {
                    return matchesInitial;
                }
            }
            return matchesInitial;
        }

        public string SolvePartOne()
        {
            var x = this.PerformIterations(1000);
            int totalEnergy = x.Select(o => o.Value.Last()).Select(x => (Math.Abs(x.x) + Math.Abs(x.y) + Math.Abs(x.z)) * (Math.Abs(x.vx) + Math.Abs(x.vy) + Math.Abs(x.vz))).Sum();
            return totalEnergy.ToString();
        }

        public string SolvePartTwo()
        {
            var x = this.FindCycle(10000000);
            // Now we know the repeating cycles of each variable. Find the Least Common Multipliers for these cycles.
            return x.Count.ToString();
        }
    }
}
