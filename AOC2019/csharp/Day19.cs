using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day19 : IDay
    {
        private readonly string data;
        private List<long> values;

        public Day19()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input19.txt"));
            this.values = this.data.Trim().Split(',',options:StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(long.Parse);
        }

        public string SolvePartOne()
        {
            var map = runProgram(50, 50);
            return map.Select(x => x.Value).Sum().ToString();
        }

        public string SolvePartTwo()
        {
            // Linear Search for fitting place...
            var x = 0;
            for (int y = 100; y < 100000; y++)
            {
                while (!CheckIfBeam(x,y))
                {
                    x += 1;
                }
                if (CheckIfBeam(x+99, y-99))
                {
                    return (10000*x + (y-99)).ToString();
                }
            }
            return "";
        }
        
        private bool CheckIfBeam(int x, int y)
        {
            IntCodeVMDay19 computer = new IntCodeVMDay19(this.values, new List<long>() {x,y});
            computer.Process();
            return computer.outputs.Last() == 1;

        }

        private Dictionary<(int x, int y), int> runProgram(int xMax, int yMax, int xMin=0, int yMin=0)
        {
            Dictionary<(int x, int y), int> map = new Dictionary<(int x, int y), int>();
            for (int x = xMin; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {   
                    if (CheckIfBeam(x,y))
                    {
                        map[(x,y)] = 1;
                        //Console.Write("#");
                    }
                    //else {Console.Write(".");}
                }
                //Console.WriteLine();
            }
            return map;
        }
    }

    public class IntCodeVMDay19
    {
        public Dictionary<int, long> memory;
        public List<long> input;
        public List<long> outputs;
        private int iPointer;
        public bool isDone;
        private long relativeBase;
        internal bool isWaitingForInput;

        public IntCodeVMDay19(List<long> program, List<long> inputValues)
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
