using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace AOC2019
{
    class Day22 : IDay
    {
        private readonly string data;
        private readonly string[] instructions;

        public Day22()
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            this.data = File.ReadAllText(Path.Combine(aocFolder, "input22.txt"));
            //this.data = "cut -4\ndeal with increment 7\ndeal into new stack";
            this.instructions = this.data.Trim().Split('\n');
        }

        private IEnumerable<int> DealIntoNewStack(IEnumerable<int> stack)
        {
            return stack.Reverse();
        }

        private IEnumerable<int> Cut(IEnumerable<int> stack, int n)
        {
            if (n < 0)
            {
                return stack.Reverse().Take(Math.Abs(n)).Reverse().Concat(stack.Take(stack.Count() - Math.Abs(n)));
            }
            else
            {
                return stack.Skip(n).Concat(stack.Take(n));
            }
        }

        private IEnumerable<int> DealWithIncrement(IEnumerable<int> stack, int n)
        {
            var length = stack.Count();
            var newStack = new int[length];
            foreach ((int value, int i) in stack.Select((value, i) => ( value, i )))
            {
                newStack[(i * n) % length] = value;
            }
            return newStack;
        }

        public string SolvePartOne()
        {
            var cards = Enumerable.Range(0,10007);
            foreach(var instruction in instructions)
            {
                if (instruction.StartsWith("deal into new stack"))
                {
                    cards = DealIntoNewStack(cards);
                }
                else if (instruction.StartsWith("cut"))
                {
                    var n = int.Parse(instruction.Replace("cut", "").Trim());
                    cards = Cut(cards, n);
                }
                else if (instruction.StartsWith("deal with increment"))
                {
                    var n = int.Parse(instruction.Replace("deal with increment", "").Trim());
                    cards = DealWithIncrement(cards, n);
                }
            }
            //Console.WriteLine($"Result: {String.Join(" ", cards)}");
            return cards.ToList().IndexOf(2019).ToString();
        }


        /// This solution is adapted from a Reddit solution...
        public string SolvePartTwo()
        {
            BigInteger c = 119315717514047;
            BigInteger n = 101741582076661;
            int p = 2020;

            BigInteger o = 0;
            BigInteger i = 1;

            foreach(var instruction in instructions)
            {
                if (instruction.StartsWith("deal into new stack"))
                {
                    o -= i;
                    i *= -1;
                }
                else if (instruction.StartsWith("cut"))
                {
                    var cutN = BigInteger.Parse(instruction.Replace("cut", "").Trim());
                    o += i * cutN;
                }
                else if (instruction.StartsWith("deal with increment"))
                {
                    var dealN = BigInteger.Parse(instruction.Replace("deal with increment", "").Trim());
                    i *= BigInteger.ModPow(dealN, c - 2, c);
                }
            }
            o *= BigInteger.ModPow(1-i, c - 2, c);
            i = BigInteger.ModPow(i, n, c);
            var value = (p*i + (1-i)*o) % c;
            return value.ToString();
        }

    }
}
