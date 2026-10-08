using Nasa.MarsRoboticRover.Application;
using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Test.Application;
using System;
using Xunit;

namespace Nasa.MarsRoboticRover.Test.Acceptance
{
    /// <summary>The example from the README, run through the real parser and executor.</summary>
    public class SampleMissionAcceptanceTests
    {
        private const string BriefInput = "5 5\n1 2 N\nLMLMLMLMM\n3 3 E\nMMRMMRMRRM";

        private static readonly string BriefOutput = string.Join(Environment.NewLine, "1 3 N", "5 1 E", "");

        private readonly IParser _parser = TestParsers.CreateDefault();
        private readonly IMissionExecutor _missionExecutor = new MissionExecutor();

        [Fact]
        public void TheBriefsExampleInput_ProducesTheBriefsExpectedOutput()
        {
            Assert.Equal(BriefOutput, _missionExecutor.Execute(_parser.Parse(BriefInput)));
        }

        [Fact]
        public void TheBuiltInSample_IsTheBriefsExample()
        {
            Assert.Equal(BriefInput, new SampleMissionInputProvider().GetInput());
        }
    }
}
