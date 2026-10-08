using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Nasa.MarsRoboticRover.Test.Presentation
{
    public class MissionRunnerTests
    {
        private static ServiceProvider BuildProvider()
        {
            return new ServiceCollection().AddMarsRover().BuildServiceProvider();
        }

        [Fact]
        public void Run_WithDefaultRegistrations_ProducesTheSampleReport()
        {
            using ServiceProvider provider = BuildProvider();

            string report = provider.GetRequiredService<MissionRunner>().Run();

            string expected = string.Join(Environment.NewLine,
                "Test Input:", "5 5", "1 2 N", "LMLMLMLMM", "3 3 E", "MMRMMRMRRM", "", "",
                "Expected Output:", "1 3 N", "5 1 E", "");
            Assert.Equal(expected, report);
        }

        [Fact]
        public void Run_Repeatedly_ProducesTheSameReport()
        {
            using ServiceProvider provider = BuildProvider();
            MissionRunner runner = provider.GetRequiredService<MissionRunner>();

            Assert.Equal(runner.Run(), runner.Run());
        }
    }
}
