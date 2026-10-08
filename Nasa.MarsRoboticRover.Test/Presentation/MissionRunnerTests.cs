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
        public void Run_WithDefaultRegistrations_ProducesTheReportWithoutDecoration()
        {
            using ServiceProvider provider = BuildProvider();

            string report = provider.GetRequiredService<MissionRunner>().Run();

            Assert.Equal(string.Join(Environment.NewLine, "1 3 N", "5 1 E", ""), report);
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
