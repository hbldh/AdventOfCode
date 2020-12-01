using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AOC2019
{
    class Day25 : IDay
    {
        private readonly string data;

        public List<long> values { get; }


        public Day25()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input25.txt"));
            //this.data = "cut -4\ndeal with increment 7\ndeal into new stack";
            this.values = this.data.Trim().Split(',',options:StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(long.Parse);
        }

        public string SolvePartOne()
        {
            var computer = new IntCodeVMDay25(this.values, new List<long>());
            computer.Process();

            var moves = new List<(string command, (int x, int y) move)>() {("north\n", (0,-1)),("south\n", (0, 1)),("west\n", (-1,0)),("east\n", (1,0))};
            var random = new Random();
            var map = new Dictionary<(int x, int y), string>();

            // Random Walk
            var position = (x: 0, y: 0);
            map[(position.x, position.y)] = computer.GetOutput();
            //bool isFound = false;
            string lastOutput;
            while (true)
            {
                if (computer.isWaitingForInput)
                {
                    var move = moves[random.Next(moves.Count)];
                    computer.SendCommand(move.command);
                    lastOutput = computer.GetOutput();
                    if (lastOutput.Contains("Analysis complete!"))
                    {
                        var regex = new Regex("typing ([0-9]*)", RegexOptions.Singleline);
                        return regex.Match(lastOutput).Groups[1].ToString();
                    }
                    if (lastOutput == "\n\n\n== Pressure-Sensitive Floor ==\nAnalyzing...\n\nDoors here lead:\n- west\n\nA loud, robotic voice says \"Alert! Droids on this ship are lighter than the detected value!\" and you are ejected back to the checkpoint.\n\n\n\n== Security Checkpoint ==\nIn the next room, a pressure-sensitive floor will verify your identity.\n\nDoors here lead:\n- north\n- east\n\nCommand?\n" )
                    {
                        //Console.WriteLine(lastOutput);
                        continue;
                    }
                    if (lastOutput == "\n\n\n== Pressure-Sensitive Floor ==\nAnalyzing...\n\nDoors here lead:\n- west\n\nA loud, robotic voice says \"Alert! Droids on this ship are heavier than the detected value!\" and you are ejected back to the checkpoint.\n\n\n\n== Security Checkpoint ==\nIn the next room, a pressure-sensitive floor will verify your identity.\n\nDoors here lead:\n- north\n- east\n\nCommand?\n" )
                    {
                        //Console.WriteLine(lastOutput);
                        continue;
                    }
                    if (lastOutput != "\nYou can't go that way.\n\nCommand?\n")
                    {
                        //Console.WriteLine(lastOutput);
                        // if (lastOutput.Contains("Security Checkpoint"))
                        // {
                        //     computer.SendCommand($"inv\n");
                        //     var gotten = computer.GetOutput();
                        //     Console.WriteLine(gotten);
                        // }
                        position = (position.x + move.move.x, position.y + move.move.y);
                        map[(position.x, position.y)] = lastOutput;
                        var regex = new Regex("Items here:(.*)\nCommand?",
                            RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        var match = regex.Match(lastOutput);
                        if (match.Success)
                        {
                            var itemsHere = match.Groups[1].ToString().Trim().Split("\n").Select(x => x.Trim().Replace("- ", "")).ToList();
                            foreach (string item in itemsHere)
                            {
                                // Dangerous items.
                                if (item == "photons" || item == "infinite loop" || item == "molten lava" || item == "escape pod" || item == "giant electromagnet")
                                {
                                    continue;
                                }
                                // Too heavy by themselves
                                if (item == "mug" || item == "prime number" || item == "weather machine")
                                {
                                    continue;
                                }
                                // Unwanted items
                                if (item == "festive hat")
                                {
                                    continue;
                                }
                                // Desired items
                                if (item == "boulder" || item == "sand" || item.Contains("ice cream") || item == "mutex")
                                {
                                    computer.SendCommand($"take {item}\n");
                                    var gotten = computer.GetOutput();
                                    //Console.WriteLine(gotten);
                                }
                            }
                        }
                    }
                }
                else if (computer.isDone)
                {
                    break;
                }
                else
                {
                    computer.Process();
                }
            }

            return computer.GetOutput();
        }

        public string SolvePartTwo()
        {
            return "";
        }
    }

    public class IntCodeVMDay25
    {
        public Dictionary<int, long> memory;
        public List<long> input;
        public List<long> outputs;
        private int iPointer;
        public bool isDone;
        private long relativeBase;
        internal bool isWaitingForInput;

        public IntCodeVMDay25(List<long> program, List<long> inputValues)
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

        public void SendCommand(string command)
        {
            this.outputs.Clear();
            this.input.AddRange(command.ToCharArray().Select(i => (long) i));
            this.Process();
        }

        public string GetOutput(bool clear=true)
        {
            var str = String.Join("", this.outputs.Select(x => (char)x));
            if (clear)
            {
                this.outputs.Clear();
            }
            return str;
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
