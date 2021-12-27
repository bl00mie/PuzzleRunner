using HarmonyLib;
using Microsoft.Extensions.Configuration;
using PuzzleRunner.Caching;
using PuzzleRunner.Config;
using PuzzleRunner.Solution;
using System.Reflection;

namespace PuzzleRunner
{
    class Program
    {
        static IConfiguration Config = InitConfig();
        static void Main(string[] args)
        {
            List<(MethodInfo m, CacheAttribute? a)> cacheMethods = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => typeof(SolutionBase).IsAssignableFrom(t)))
                .SelectMany(t => t.GetMethods().Where(m => m.GetCustomAttribute(typeof(Caching.CacheAttribute)) != null))
                .Select(m => (m, m.GetCustomAttribute(typeof(CacheAttribute)) as CacheAttribute))
                .ToList();
            var harmony = new Harmony("PuzzleRunner");

            foreach (var (m, a) in cacheMethods)
            {
                var mPrefix = typeof(MethodCache).GetMethod("MethodPrefix");
                var mPostfix = typeof(MethodCache).GetMethod("MethodPostfix");
                harmony.Patch(m, new HarmonyMethod(mPrefix), new HarmonyMethod(mPostfix));
            }

            InitConfig();
            var section = Config.GetSection(AoCSection.SectionName);
            var aocConfig = section.Get<AoCSection>();
            var runner = new AoCRunner(aocConfig, "2021", "24");
            var input = runner.RetrieveInputAsync(Config).Result;
            runner.RunSolution(input);
        }



        static IConfiguration InitConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }

}