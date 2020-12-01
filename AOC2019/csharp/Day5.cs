using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day5 : IDay
    {
        private readonly string data;
        private List<int> values;

        public Day5()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input05.txt"));
            this.values = this.data.Split(',',options:StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(int.Parse);
        }

        public string SolvePartOne()
        {
            var c = new IntCodeComputerDay5(this.values, 1);
            c.Process();
            return c.outputs.Last().ToString();
        }

        public string SolvePartTwo()
        {
            var c = new IntCodeComputerDay5(this.values, 5);
            c.Process();
            return c.outputs.Last().ToString();
        }
    }

    public class Instruction
    {
        public int Opcode;
        public int FirstParamMode;
        public int SecondParamMode;
        public int ThirdParamMode;

        public Instruction(int val)
        {
            string instr = val.ToString().PadLeft(5,'0');
            this.Opcode = int.Parse(instr.Substring(3));
            this.FirstParamMode = int.Parse(instr.Substring(2,1));
            this.SecondParamMode = int.Parse(instr.Substring(1,1));
            this.ThirdParamMode = int.Parse(instr.Substring(0,1));
        }
    }

    public class IntCodeComputerDay5
    {
        public List<int> memory;
        public int input;
        public List<int> outputs;
        private int iPointer;
        public IntCodeComputerDay5(List<int> input, int inputValue)
        {
            this.memory = new List<int>(input);
            this.input = inputValue;
            this.outputs = new List<int>();
            this.iPointer = 0;
        }

        public void Process()
        {
            
            var instructionLength = 0;
            bool running = true;
            while (running)
            {
                var instruction = new Instruction(this.memory[iPointer]);
                switch (instruction.Opcode)
                {
                    case 1:
                        OpCode1(instruction, iPointer);
                        instructionLength = 4;
                        break;
                    case 2:
                        OpCode2(instruction, iPointer);
                        instructionLength = 4;
                        break;
                    case 3:
                        OpCode3(instruction, iPointer);
                        instructionLength = 2;
                        break;
                    case 4:
                        OpCode4(instruction, iPointer);
                        instructionLength = 2;
                        break;
                    case 5:
                        instructionLength = OpCode5(instruction, iPointer);
                        break;
                    case 6:
                        instructionLength = OpCode6(instruction, iPointer);
                        break;
                    case 7:
                        instructionLength = OpCode7(instruction, iPointer);
                        break;
                    case 8:
                        instructionLength = OpCode8(instruction, iPointer);
                        break;
                    case 99:
                        running = false;
                        instructionLength = 0;
                        break;
                    default:
                        throw new Exception($"Incorrect OpCode {this.memory[iPointer]}");
                }
                this.iPointer += instructionLength;
            }
        }  
        

        /// Opcode 5 is jump-if-true: if the first parameter is non-zero, it sets the instruction pointer to the 
        /// value from the second parameter. Otherwise, it does nothing.
        private int OpCode5(Instruction instruction, int p)
        {
            int addressOne = instruction.FirstParamMode == 1 ? p + 1 : this.memory[p + 1];
            int addressTwo = instruction.SecondParamMode == 1 ? p + 2 : this.memory[p + 2];
            if (this.memory[addressOne] != 0)
            {
                this.iPointer = this.memory[addressTwo];
                return 0;
            }
            else
            {
                return 3;
            }
        }
        
        ///  Opcode 6 is jump-if-false: if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
        private int OpCode6(Instruction instruction, int p)
        {
            int addressOne = instruction.FirstParamMode == 1 ? p + 1 : this.memory[p + 1];
            int addressTwo = instruction.SecondParamMode == 1 ? p + 2 : this.memory[p + 2];
            if (this.memory[addressOne] == 0)
            {
                this.iPointer = this.memory[addressTwo];
                return 0;
            }
            else
            {
                return 3;
            }
        }
        /// Opcode 7 is less than: if the first parameter is less than the second parameter, it stores 1 in the position given 
        // by the third parameter. Otherwise, it stores 0.
        private int OpCode7(Instruction instruction, int p)
        {
            int addressOne = instruction.FirstParamMode == 1 ? p + 1 : this.memory[p + 1];
            int addressTwo = instruction.SecondParamMode == 1 ? p + 2 : this.memory[p + 2];
            int addressThree = instruction.ThirdParamMode == 1 ? p + 3 : this.memory[p + 3];
            if (this.memory[addressOne] < this.memory[addressTwo])
            {
                this.memory[addressThree] = 1;
            }
            else
            {
                this.memory[addressThree] = 0;
            }
            return 4;
        }

        /// Opcode 8 is equals: if the first parameter is equal to the second parameter, it stores 1 in the position 
        /// given by the third parameter. Otherwise, it stores 0.
        private int OpCode8(Instruction instruction, int p)
        {
            int addressOne = instruction.FirstParamMode == 1 ? p + 1 : this.memory[p + 1];
            int addressTwo = instruction.SecondParamMode == 1 ? p + 2 : this.memory[p + 2];
            int addressThree = instruction.ThirdParamMode == 1 ? p + 3 : this.memory[p + 3];
            if (this.memory[addressOne] == this.memory[addressTwo])
            {
                this.memory[addressThree] = 1;
            }
            else
            {
                this.memory[addressThree] = 0;
            }
            return 4;
        }


        /// Opcode 4 outputs the value of its only parameter. For example, the instruction 4,50 would output the value at address 50.
        private int OpCode4(Instruction instruction, int p)
        {
            int addressOne = instruction.FirstParamMode == 1 ? p + 1 : this.memory[p + 1];
            this.outputs.Add(this.memory[addressOne]);
            return 2;
        }

        /// Opcode 3 takes a single integer as input and saves it to the position given by its only parameter. 
        /// For example, the instruction 3,50 would take an input value and store it at address 50.
        private int OpCode3(Instruction instruction, int p)
        {
            int addressOne = instruction.FirstParamMode == 1 ? p + 1 : this.memory[p + 1];
            this.memory[addressOne] = this.input;
            return 2;
        }

        private int OpCode2(Instruction instruction, int p)
        {
            int addressOne = instruction.FirstParamMode == 1 ? p + 1 : this.memory[p + 1];
            int addressTwo = instruction.SecondParamMode == 1 ? p + 2 : this.memory[p + 2];
            int addressThree = instruction.ThirdParamMode == 1 ? p + 3 : this.memory[p + 3];
            this.memory[addressThree] = (this.memory[addressOne] * this.memory[addressTwo]);
            return 4;
        }

        private int OpCode1(Instruction instruction, int p)
        {
            int addressOne = instruction.FirstParamMode == 1 ? p + 1 : this.memory[p + 1];
            int addressTwo = instruction.SecondParamMode == 1 ? p + 2 : this.memory[p + 2];
            int addressThree = instruction.ThirdParamMode == 1 ? p + 3 : this.memory[p + 3];
            this.memory[addressThree] = (this.memory[addressOne] + this.memory[addressTwo]);
            return 4;
        }
    }
}
