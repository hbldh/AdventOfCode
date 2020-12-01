using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day15 : IDay
    {
        private readonly string data;

        public List<long> values { get; }

        private Dictionary<(int x, int y), string> map;

        private (int x, int y) size;

        public Day15()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input15.txt"));
            this.values = this.data.Trim().Split(',',options:StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(long.Parse);
        }

        public string SolvePartOne()
        {
            this.map = new Dictionary<(int x, int y), string>();
            var random = new Random();
            var moves = new List<(int command, (int x, int y) move)>() {(1, (0,-1)),(2, (0, 1)),(3, (-1,0)),(4, (1,0))};
            var droid = new IntCodeVMDay15(this.values, new List<long>());
            droid.Process();
            var position = (x: 0, y: 0);
            this.map[(position.x, position.y)] = "O";
            bool isFound = false;
            while (!isFound)
            {
                if (droid.isWaitingForInput)
                {
                    var move = moves[random.Next(moves.Count)];
                    droid.input.Add(move.command);
                    droid.Process();
                    switch (droid.outputs.Last())
                    {
                        case 0:
                            this.map[(position.x + move.move.x, position.y + move.move.y)] = "#";
                            break;
                        case 1:
                            position = (position.x + move.move.x, position.y + move.move.y);
                            this.map[position] = position.x == 0 && position.y == 0 ? "O" : ".";
                            break;
                        case 2:
                            position = (position.x + move.move.x, position.y + move.move.y);
                            this.map[position] = "X";
                            isFound = true;
                            break;
                    }
                }
                else
                {
                    droid.Process();
                }
            }
            
            var states = new List<(int x, int y, HashSet<(int x, int y)> visited)>();
            states.Add((0,0, new HashSet<(int x, int y)>() { (0,0) }));
            while (states.Count > 0)
            {
                var currentState = states.First();
                states.RemoveAt(0);
                foreach (var move in moves)
                {
                    var newposition = (x: currentState.x + move.move.x, y: currentState.y + move.move.y);
                    if (currentState.visited.Contains(newposition))
                    {
                        continue;
                    }
                    if (this.map.ContainsKey(newposition))
                    {
                        if (this.map[newposition] == ".")
                        {
                            var visited = new HashSet<(int x, int y)>(currentState.visited);
                            visited.Add(newposition);
                            states.Add((newposition.x, newposition.y, visited));
                        }
                        else if (this.map[newposition] == "X")
                        {
                            return (currentState.visited.Count).ToString();
                        }
                    }
                }
                states.Sort((a,b) => a.visited.Count.CompareTo(b.visited.Count));
            }

            return "";
        }

        private void PrintMap(){
            var xMin = this.map.Keys.Select(x => x.x).Min();
            var xMax = this.map.Keys.Select(x => x.x).Max();
            var yMin = this.map.Keys.Select(x => x.y).Min();
            var yMax = this.map.Keys.Select(x => x.y).Max();
            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    if (this.map.ContainsKey((x, y)))
                    {
                        Console.Write(this.map[(x, y)]);
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }

        public string SolvePartTwo()
        {
            var moves = new List<(int command, (int x, int y) move)>() {(1, (0,-1)),(2, (0, 1)),(3, (-1,0)),(4, (1,0))};
        
            var states = new List<(int x, int y, HashSet<(int x, int y)> visited)>();
            var OPosition = this.map.Where(x => x.Value == "O").First().Key;
            states.Add((OPosition.x, OPosition.y, new HashSet<(int x, int y)>() { OPosition }));
            var distances = new Dictionary<(int x, int y), int>();
            while (states.Count > 0)
            {
                var currentState = states.First();
                states.RemoveAt(0);
                foreach (var move in moves)
                {
                    var newposition = (x: currentState.x + move.move.x, y: currentState.y + move.move.y);
                    if (currentState.visited.Contains(newposition))
                    {
                        continue;
                    }
                    if (this.map.ContainsKey(newposition))
                    {
                        if (this.map[newposition] == "#")
                        {
                            continue;
                        }
                        else
                        {
                            var visited = new HashSet<(int x, int y)>(currentState.visited);
                            visited.Add(newposition);
                            states.Add((newposition.x, newposition.y, visited));
                            distances[newposition] = Math.Min(currentState.visited.Count, distances.GetValueOrDefault(newposition, 100000));
                        }
                    }
                }
                states.Sort((a,b) => a.visited.Count.CompareTo(b.visited.Count));
            }

            return distances.Select(x => x.Value).Max().ToString();
        }

    }

    public class IntCodeVMDay15
    {
        public Dictionary<int, long> memory;
        public List<long> input;
        public List<long> outputs;
        private int iPointer;
        public bool isDone;
        private long relativeBase;
        internal bool isWaitingForInput;

        public IntCodeVMDay15(List<long> program, List<long> inputValues)
        {
            this.memory = new Dictionary<int, long>(){};
            for (int i = 0; i < program.Count; i++)
            {
                this.memory[i] = program[i];
            }
            this.input = inputValues;
            this.outputs = new List<long>();
            this.iPointer = 0;
            this.isDone = false;
            this.relativeBase = 0;
            this.isWaitingForInput = false;
        }

        private void Write(int address, long value)
        {
            this.memory[address] = value;
        }

        private long Read(int address)
        {
            return this.memory.GetValueOrDefault(address, 0);
        }

        public void Process()
        {
            var sizes = new List<int>() {0, 4, 4, 2, 2, 3, 3, 4, 4, 2};
            int[] modesList = new int[3] {2,3,4};
            while (!this.isDone)
            {   
                var operation = (int)(this.Read(this.iPointer) % 100);
                if (operation == 99)
                {
                    this.isDone = true;
                    break;
                }
                List<long> args = new List<long>();
                for (int j = this.iPointer + 1; j < this.iPointer+sizes[operation]; j++)
                {
                    args.Add(this.memory[j]);
                }
                // [(mem[ip] // 10 ** i) % 10 for i in range(2, 5)]
                var modes = modesList.Select(i => (int)((this.Read(iPointer)) / Math.Pow(10, i)) % 10).ToArray();
                var reads = args.Zip(modes, (x, m)=>(new List<long>(){this.Read((int)x), x, this.Read((int)(this.relativeBase + x))})[m]).ToArray();
                var writes = args.Zip(modes, (x, m)=>(new List<long>(){x, 0, this.relativeBase + x})[m]).ToArray();
                this.iPointer += sizes[operation];
                switch (operation)
                {
                    case 1:
                        this.Write((int)writes[2], reads[0] + reads[1]);
                        break;
                    case 2:
                        this.Write((int)writes[2], reads[0] * reads[1]);
                        break;
                    case 3:
                        this.isWaitingForInput = false;
                        if (this.input.Count == 0)
                        {
                            this.iPointer -= sizes[operation];
                            this.isWaitingForInput = true;
                            return;
                        }
                        this.Write((int)writes[0], this.input[0]);
                        this.input.RemoveAt(0);
                        break;
                    case 4:
                        this.outputs.Add(reads[0]);
                        return;
                    case 5:
                        this.iPointer = (int)(reads[0] != 0 ? reads[1] : this.iPointer);
                        break;
                    case 6:
                        this.iPointer = (int)(reads[0] == 0 ? reads[1] : this.iPointer);
                        break;
                    case 7:
                        this.Write((int)writes[2], reads[0] < reads[1] ? 1 : 0);
                        break;
                    case 8:
                        this.Write((int)writes[2], reads[0] == reads[1] ? 1 : 0);
                        break;
                    case 9:
                        this.relativeBase += reads[0];
                        break;
                }
            }
        }
    }
}
