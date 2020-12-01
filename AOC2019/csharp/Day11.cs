using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day11 : IDay
    {
        private readonly string data;
        private List<long> values;

        public Day11()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input11.txt"));
            this.values = this.data.Trim().Split(',',options:StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(long.Parse);
        }

        public string SolvePartOne()
        {
            var hull = paintHull(0);
            return hull.Count.ToString();
        }

        public string SolvePartTwo()
        {
            var hull = paintHull(1);
            var xMin = hull.Keys.Min(o => o.Item1);
            var xMax = hull.Keys.Max(o => o.Item1);
            var yMin = hull.Keys.Min(o => o.Item2);
            var yMax = hull.Keys.Max(o => o.Item2);

            var reg = new List<List<string>>();
            for (int y = yMin; y <= yMax; y++)
            {
                var row = new List<string>();
                for (int x = xMin; x <= xMax; x++)
                {
                    var color = hull.GetValueOrDefault(new Tuple<int, int>(x, y), 0);
                    row.Add(color == 1 ? "#" : " ");
                }
                reg.Add(row);
            }
            reg.Reverse();
            foreach (var row in reg)
            {
                Console.WriteLine(string.Concat(row));
            }
            return "KRZEAJHB";
        }
        
        private Dictionary<Tuple<int, int>, int> paintHull(int startColor)
        {
            Dictionary<Tuple<int, int>, int> hull = new Dictionary<Tuple<int, int>, int>() {{new Tuple<int, int>(0,0), 0}};
            int direction = 0;
            List<Tuple<int, int>> directions = new List<Tuple<int, int>>() {new Tuple<int, int>(0,1), new Tuple<int, int>(1,0), new Tuple<int, int>(0,-1), new Tuple<int, int>(-1,0)};
            Tuple<int, int> position = new Tuple<int, int>(0,0);
            IntCodeVMDay11 computer = new IntCodeVMDay11(this.values, new List<long>() {startColor, });
            while (!computer.isDone)
            {
                computer.Process();
                if (computer.isDone)
                {
                    break;
                }
                // Get new color.
                hull[position] = (int)computer.outputs.Last();
                
                // Get new direction.
                computer.Process();
                direction += computer.outputs.Last() == 1 ? 1 : -1;
                direction = direction < 0 ? 3 : direction % 4;
                position = new Tuple<int, int>(position.Item1 + directions[direction].Item1, position.Item2 + directions[direction].Item2);
                // Send new position's colour to computer as input.
                computer.input.Add((long)hull.GetValueOrDefault(position, 0));
            }
            return hull;
        }
    }

    public class IntCodeVMDay11
    {
        public Dictionary<int, long> memory;
        public List<long> input;
        private Dictionary<Tuple<int, int>, int> hull;
        public List<long> outputs;
        private int iPointer;
        public bool isDone;
        private long relativeBase;

        public IntCodeVMDay11(List<long> program, List<long> inputValues)
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
            bool haltOnOutput = false;
            var sizes = new List<int>() {0, 4, 4, 2, 2, 3, 3, 4, 4, 2};
            int[] modesList = new int[3] {2,3,4};
            while (!haltOnOutput && !this.isDone)
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
                        this.Write((int)writes[0], this.input[0]);
                        this.input.RemoveAt(0);
                        break;
                    case 4:
                        this.outputs.Add(reads[0]);
                        haltOnOutput = true;
                        break;
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
