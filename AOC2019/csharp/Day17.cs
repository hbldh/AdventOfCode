using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day17 : IDay
    {
        private readonly string data;
        private List<long> values;

        public Day17()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input17.txt"));
            this.values = this.data.Trim().Split(',',options:StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(long.Parse);
        }

        public string SolvePartOne()
        {
            var scaffolding = runProgram(new List<long>());
            var intersections = GetIntersections(scaffolding);
            return intersections.Select(s => s.x * s.y).Sum().ToString();
        }

        public string SolvePartTwo()
        {
            // Solved it by hand...
            var codes = new Dictionary<string, string>() {
                {"A", "L,4,L,6,L,8,L,12\n"},
                {"B",  "L,8,R,12,L,12\n"},
                {"C",  "R,12,L,6,L,6,L,8\n"}
            };
            var sequence = "A,B,B,A,B,C,A,C,B,C\n";

            var input = sequence.ToCharArray().Select(i => (long)i).ToList();
            input.AddRange(codes["A"].ToCharArray().Select(i => (long)i).ToList());
            input.AddRange(codes["B"].ToCharArray().Select(i => (long)i).ToList());
            input.AddRange(codes["C"].ToCharArray().Select(i => (long)i).ToList());
            input.AddRange("n\n".ToCharArray().Select(i => (long)i).ToList());
            IntCodeVMDay17 computer = new IntCodeVMDay17(this.values, input);
            computer.memory[0] = 2;
            while (!computer.isDone)
            {
                computer.Process();
            }
            return computer.outputs.Last().ToString();
        }
        
        private Dictionary<(int x, int y), int> runProgram(List<long> input)
        {
            Dictionary<(int x, int y), int> scaffolding = new Dictionary<(int x, int y), int>();
            IntCodeVMDay17 computer = new IntCodeVMDay17(this.values, input);
            int row = 0;
            int col = 0;
            while (!computer.isDone)
            {
                computer.Process();
                switch (computer.outputs.Last())
                {
                    case 35:
                        scaffolding[(col, row)] = 1;
                        col += 1;
                        Console.Write("#");
                        break;
                    case 46:
                        scaffolding[(col, row)] = 0;
                        col += 1;
                        Console.Write(".");
                        break;
                    case 10:
                        row += 1;
                        col = 0;
                        Console.WriteLine("");
                        break;
                }
            }
            return scaffolding;
        }
        private List<(int x, int y)> GetIntersections(Dictionary<(int x, int y), int> scaffolding)
        {
            var intersections = new List<(int x, int y)>();
            foreach (var s in scaffolding.Where(x => x.Value == 1))
            {
                if (scaffolding.GetValueOrDefault((s.Key.x+1, s.Key.y), 0) == 1 && scaffolding.GetValueOrDefault((s.Key.x-1, s.Key.y), 0) == 1 & scaffolding.GetValueOrDefault((s.Key.x, s.Key.y + 1), 0) == 1 && scaffolding.GetValueOrDefault((s.Key.x, s.Key.y-1), 0) == 1)
                {
                    intersections.Add(s.Key);
                }
            }
            return intersections;
        }
    }

    public class IntCodeVMDay17
    {
        public Dictionary<int, long> memory;
        public List<long> input;
        public List<long> outputs;
        private int iPointer;
        public bool isDone;
        private long relativeBase;
        internal bool isWaitingForInput;

        public IntCodeVMDay17(List<long> program, List<long> inputValues)
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
