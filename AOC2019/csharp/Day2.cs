using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day2 : IDay
    {
        private readonly string data;
        private List<int> values;

        public Day2()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input02.txt"));
            this.values = this.data.Split(',',options:StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(int.Parse);
        }

        public string SolvePartOne()
        {
            var computer = new IntCodeComputerDay2(this.values, 12, 2);
            computer.Process();
            return computer.memory[0].ToString();
        }

        public string SolvePartTwo()
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    var computer = new IntCodeComputerDay2(this.values, i, j);
                    computer.Process();
                    if (computer.memory[0] == 19690720)
                    {
                        return (100 * i + j).ToString();
                    }
                }
            }
            return "0";
        }
    }

        public class IntCodeComputerDay2
    {
        public List<int> memory;
        public IntCodeComputerDay2(List<int> input, int? noun=null, int? verb=null)
        {
            this.memory = new List<int>(input);
            if (noun != null)
            {
                this.memory[1] = (int)noun;
            }
            if (verb != null)
            {
                this.memory[2] = (int)verb;
            }
        }

        public void Process()
        {
            int iPointer = 0;
            var instructionLength = 0;
            bool running = true;
            while (running)
            {
                
                switch (this.memory[iPointer])
                {
                    case 1:
                        instructionLength = OpCode1(iPointer);
                        break;
                    case 2:
                        instructionLength = OpCode2(iPointer);
                        break;
                    case 99:
                        running = false;
                        instructionLength = 0;
                        break;
                    default:
                        throw new Exception($"Incorrect OpCode {this.memory[iPointer]}");
                }
                iPointer = (iPointer + instructionLength);
            }
        }

        private int OpCode1(int p)
        {
            this.memory[this.memory[p + 3]] = (this.memory[this.memory[p + 1]] + this.memory[this.memory[p + 2]]);
            return 4;
        }
        private int OpCode2(int p)
        {
            this.memory[this.memory[p + 3]] = (this.memory[this.memory[p + 1]] * this.memory[this.memory[p + 2]]);
            return 4;
        }
    }
}
