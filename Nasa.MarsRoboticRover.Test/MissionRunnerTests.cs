using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Nasa.MarsRoboticRover.Test
{
    public class MissionRunnerTests
    {
        [Fact]
        public void Run_WithDefaultRegistrations_ProducesTheSampleReport()
        {
            using ServiceProvider provider = new ServiceCollection().AddMarsRover().BuildServiceProvider(validateScopes: true);
            using IServiceScope scope = provider.CreateScope();

            string report = scope.ServiceProvider.GetRequiredService<MissionRunner>().Run();

            string expected = string.Join(Environment.NewLine,
                "Test Input:", "5 5", "1 2 N", "LMLMLMLMM", "3 3 E", "MMRMMRMRRM", "", "",
                "Expected Output:", "1 3 N", "5 1 E", "");
            Assert.Equal(expected, report);
        }

        [Fact]
        public void Run_InSeparateScopes_DoesNotShareMissionState()
        {
            using ServiceProvider provider = new ServiceCollection().AddMarsRover().BuildServiceProvider(validateScopes: true);

            string first, second;
            using (IServiceScope scope = provider.CreateScope())
                first = scope.ServiceProvider.GetRequiredService<MissionRunner>().Run();
            using (IServiceScope scope = provider.CreateScope())
                second = scope.ServiceProvider.GetRequiredService<MissionRunner>().Run();

            Assert.Equal(first, second);
        }
    }
}
