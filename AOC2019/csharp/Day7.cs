using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AOC2019
{
    class Day7 : IDay
    {
        private readonly string data;
        private List<int> values;

        public Day7()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input07.txt"));
            this.values = this.data.Split(',',options:StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(int.Parse);
        }

        public string SolvePartOne()
        {
            var combos = Permutations(new List<int>() {0,1,2,3,4});
            var results = new List<int>();
            
            foreach (var combo in combos)
            {
                var input = 0;
                foreach (var phaseValue in combo)
                {
                    var c = new IntCodeComputerDay7(this.values, input, phaseValue);
                    c.Process();
                    input = c.outputs.Last();
                }
                results.Add(input);
            }
            
            return results.Max().ToString();
        }

        public string SolvePartTwo()
        {
            var combos = Permutations(new List<int>() {5,6,7,8,9});
            var results = new List<int>();
            var keepRunning = true;

            foreach (var combo in combos)
            {
                int input = 0;
                keepRunning = true;
                var computers = new List<IntCodeComputerDay7>() {
                    new IntCodeComputerDay7(this.values, 0, combo.ToList()[0], false),
                    new IntCodeComputerDay7(this.values, 0, combo.ToList()[1], false),
                    new IntCodeComputerDay7(this.values, 0, combo.ToList()[2], false),
                    new IntCodeComputerDay7(this.values, 0, combo.ToList()[3], false),
                    new IntCodeComputerDay7(this.values, 0, combo.ToList()[4], false),
                };

                while (keepRunning)
                {
                    for (int i = 0; i < computers.Count; i++)
                    {
                        computers[i].input = input;
                        computers[i].Process();
                        input = computers[i].outputs.Last();
                    }
                    if (computers.Any(o => o.isDone))
                    {
                        keepRunning = false;
                    }
                }
                results.Add(computers.Last().outputs.Last());
            }
            
            return results.Max().ToString();
        }

        public static ICollection<ICollection<T>> Permutations<T>(ICollection<T> list) {
            var result = new List<ICollection<T>>();
            if (list.Count == 1) { // If only one possible permutation
                result.Add(list); // Add it and return it
                return result;
            }
            foreach (var element in list) { // For each element in that list
                var remainingList = new List<T>(list);
                remainingList.Remove(element); // Get a list containing everything except of chosen element
                foreach (var permutation in Permutations<T>(remainingList)) { // Get all possible sub-permutations
                    permutation.Add(element); // Add that element
                    result.Add(permutation);
                }
            }
            return result;
        }

    }

    public class IntCodeComputerDay7
    {
        public List<int> memory;
        public int input;
        private readonly int phase;
        private readonly bool runUntil99;
        private bool phaseUsed;
        public List<int> outputs;
        private int iPointer;
        public bool isDone;

        public IntCodeComputerDay7(List<int> input, int inputValue, int phaseValue, bool runUntil99=true)
        {
            this.memory = new List<int>(input);
            this.input = inputValue;
            this.phase = phaseValue;
            this.runUntil99 = runUntil99;
            this.phaseUsed = false;
            this.outputs = new List<int>();
            this.iPointer = 0;
            this.isDone = false;
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
                        if (!this.runUntil99){
                            running = false;
                        }
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
                        this.isDone = true;
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
            this.memory[addressOne] = this.phaseUsed ? this.input : this.phase;
            this.phaseUsed = true;
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
