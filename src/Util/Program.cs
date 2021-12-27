using Microsoft.Extensions.Configuration;
using PuzzleRunner.Config;
using PuzzleRunner.Solution;

namespace PuzzleRunner
{
    class Program
    {
        static IConfiguration Config = InitConfig();
        static void Main(string[] args)
        {
            InitConfig();
            var section = Config.GetSection(AoCSection.SectionName);
            var aocConfig = section.Get<AoCSection>();
            AoCRunner runner = new AoCRunner(aocConfig, "2017", "1");
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