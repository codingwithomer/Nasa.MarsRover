using Microsoft.Extensions.DependencyInjection;
using Nasa.MarsRoboticRover.Application.Interfaces;
using System;
using Xunit;

namespace Nasa.MarsRoboticRover.Test.Presentation
{
    public class MissionRunnerTests
    {
        private static ServiceProvider BuildProvider()
        {
            return new ServiceCollection()
                .AddMarsRover()
                .AddSingleton<IMissionInputProvider>(new SampleMissionInputProvider())
                .BuildServiceProvider();
        }

        [Fact]
        public void Run_WithTheSampleInput_ProducesTheReportWithoutDecoration()
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

        [Fact]
        public void AddMarsRover_DoesNotRegisterAnInputProvider()
        {
            using ServiceProvider provider = new ServiceCollection().AddMarsRover().BuildServiceProvider();

            Assert.Null(provider.GetService<IMissionInputProvider>());
        }
    }
}
