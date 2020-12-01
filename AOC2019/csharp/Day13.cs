using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day13 : IDay
    {
        private readonly string data;
        private List<long> values;

        public Day13()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input13.txt"));
            this.values = this.data.Trim().Split(',',options:StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(long.Parse);
        }

        public string SolvePartOne()
        {
            var tiles = runProgram(null, new List<long>());
            return tiles.Where(o => o.Value == 2).Count().ToString();
        }

        public string SolvePartTwo()
        {
            var tiles = runProgram(2, new List<long>());
            return tiles[new Tuple<int, int>(-1,0)].ToString();
        }
        
        private Dictionary<Tuple<int, int>, int> runProgram(int? mem0, List<long> input)
        {
            Dictionary<Tuple<int, int>, int> hull = new Dictionary<Tuple<int, int>, int>();
            IntCodeVMDay13 computer = new IntCodeVMDay13(this.values, input);
            computer.memory[0] = mem0 == null ? computer.memory[0] : (int) mem0;
            while (true)
            {
                computer.Process();
                if (computer.isDone)
                {
                    break;
                }
                if (computer.isWaitingForInput)
                {
                    var ball = hull.Where(p => p.Value == 4).First().Key;
                    var paddle = hull.Where(p => p.Value == 3).First().Key;
                    if (paddle.Item1 < ball.Item1)
                        computer.input.Add(1);
                    else if (paddle.Item1 > ball.Item1)
                        computer.input.Add(-1);
                    else
                        computer.input.Add(0);
                    continue;
                }
                var x =  (int)computer.outputs.Last();
                computer.Process();
                var y =  (int)computer.outputs.Last();
                computer.Process();
                var tileId = (int)computer.outputs.Last();
                var pos = new Tuple<int, int>(x,y);
                hull[pos] = tileId;
            }
            return hull;
        }
    }

    public class IntCodeVMDay13
    {
        public Dictionary<int, long> memory;
        public List<long> input;
        private Dictionary<Tuple<int, int>, int> hull;
        public List<long> outputs;
        private int iPointer;
        public bool isDone;
        private long relativeBase;
        internal bool isWaitingForInput;

        public IntCodeVMDay13(List<long> program, List<long> inputValues)
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
