using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace AOC2019
{
    public class Executor
    {
        private readonly string sessionToken;

        public Executor(string sessionToken)
        {
            this.sessionToken = sessionToken;
        }

        public void Run(int? day)
        {
            if (day is null)
            {
                // Run all!
                for (int i = 1; i < 25; i++)
                {
                    try
                    {
                        ExecuteDay(i);
                    }
                    catch
                    {
                        // Let this pass silently...
                    }
                }
            }
            else
            {
                // Run specified day.
                ExecuteDay((int)day);
            }
        }

        private void ExecuteDay(int day)
        {
            EnsureData((int)day);
            Type dayType = Type.GetType($"AOC2019.Day{day}");
            IDay dayObj = (IDay)Activator.CreateInstance(dayType);
            Console.WriteLine($" -* Day {day} *-");


            Stopwatch t = new Stopwatch();
            t.Start();
            var x = dayObj.SolvePartOne();
            t.Stop();
            var t1 = t.Elapsed.ToString("g");
            t.Reset();
            Console.WriteLine($"Part 1: {x}");
            Console.WriteLine($"Time taken for part 1: {t1}");
            
            t.Start();
            var y = dayObj.SolvePartTwo();
            t.Stop();
            var t2 = t.Elapsed.ToString("g");
            Console.WriteLine($"Part 2: {y}");
            Console.WriteLine($"Time taken for part 2: {t2}");

            Console.WriteLine($"---------------------------");
        }

        private bool EnsureData(int day)
        {
            var aocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AOC2019");
            Directory.CreateDirectory(aocFolder);
            if (!File.Exists(Path.Combine(aocFolder, $"input{day.ToString().PadLeft(2, '0')}.txt")))
            {
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Cookie", $"session={this.sessionToken}");
                var resp = client.GetAsync($"https://adventofcode.com/2019/day/{day}/input").ConfigureAwait(false).GetAwaiter().GetResult();
                var data = resp.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                File.WriteAllText(Path.Combine(aocFolder, $"input{day.ToString().PadLeft(2, '0')}.txt"), data);
            }
            return true;
        }
    }
}
