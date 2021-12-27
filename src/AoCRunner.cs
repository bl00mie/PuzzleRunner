using Microsoft.Extensions.Configuration;
using PuzzleRunner.Config;
using PuzzleRunner.Solution;
using System.Diagnostics;
using System.Net;

namespace PuzzleRunner
{
    internal class AoCRunner
    {
        private static readonly Dictionary<(string, string), Type> Solutions = new()
        {
            [("2017", "1")] = typeof(Puzzle2017_01)
        };

        public static string PuzzleType = "aoc";
        public string Year { get; }
        public string Day { get; }
        private AoCSection AoCConfig;

        public AoCRunner() : this(new AoCSection(), string.Empty, string.Empty)
        {
        }

        public AoCRunner(AoCSection config) : this(config, string.Empty, string.Empty)
        {
        }

        public AoCRunner(AoCSection config, string year, string day)
        {
            AoCConfig = config;
            if (string.IsNullOrEmpty(year))
                if (!string.IsNullOrEmpty(AoCConfig.DefaultYear))
                    Year = AoCConfig.DefaultYear;
                else
                    Year = DateTime.Now.Year.ToString();
            else
                Year = year;

            if (string.IsNullOrEmpty(day))
                if (!string.IsNullOrEmpty(AoCConfig.DefaultDay))
                    Day = AoCConfig.DefaultDay;
                else
                    Day = DateTime.Now.Day.ToString();
            else
                Day = day;
        }

        public void RunSolution(IEnumerable<string> input)
        {
            if (Activator.CreateInstance(Solutions[new(Year, Day)]) is not AoCSolution solution)
                throw new Exception($"Could not find a solution for year:{Year} day:{Day}");

            Stopwatch sw = new();
            sw.Start();
            solution.ParseInput(input);
            sw.Stop();
            Debug.WriteLine($"Input processed in {sw.ElapsedMilliseconds} ms");
            
            sw.Restart();
            var part1 = solution.Part1();
            sw.Stop();
            if (!string.IsNullOrEmpty(part1))
                Clipboard.Copy(part1);
            Debug.WriteLine($"Part 1: {part1} ({sw.ElapsedMilliseconds})");

            sw.Restart();
            var part2 = solution.Part2();
            sw.Stop();
            Debug.WriteLine($"Part 2: {part2} ({sw.ElapsedMilliseconds})");
            if ((!string.IsNullOrEmpty(part2)))
                Clipboard.Copy(part2);
        }

        public async Task<IEnumerable<string>> RetrieveInputAsync(IConfiguration config)
        {
            var inputDir = config["inputPath"];
            if (!Directory.Exists(inputDir))
            {
                Debug.WriteLine($"Creating input directory {inputDir}");
                Directory.CreateDirectory(inputDir);
            }
            var aocDir = $"{inputDir}/AoC";
            if (!Directory.Exists(aocDir))
            {
                Debug.WriteLine($"Creating input directory {aocDir}");
                Directory.CreateDirectory(aocDir);
            }
            var filePath = $"{aocDir}/{Year}_{Day}";
            if (!File.Exists(filePath))
            {
                var baseAddress = new Uri($"https://adventofcode.com/{Year}/day/{Day}/input");
                var cookieContainer = new CookieContainer();
                using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
                using var client = new HttpClient(handler) { BaseAddress = baseAddress };
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                cookieContainer.Add(baseAddress, new Cookie("session", AoCConfig.SessionCookie));
                var response = await client.GetAsync(baseAddress);
                if (response.IsSuccessStatusCode)
                {
                    using var writer = new StreamWriter(filePath);
                    writer.Write(await response.Content.ReadAsStringAsync());
                    writer.Flush();
                }
            }
            return File.ReadLines(filePath);
        }
    }
}
