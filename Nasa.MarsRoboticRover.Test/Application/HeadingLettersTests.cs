using Nasa.MarsRoboticRover.Application;
using Xunit;
using System;

namespace Nasa.MarsRoboticRover.Test.Application
{
    public class HeadingLettersTests
    {
        [Theory]
        [InlineData("N")]
        [InlineData("E")]
        [InlineData("S")]
        [InlineData("W")]
        public void Heading_SurvivesAParseExecuteReportRoundTrip(string heading)
        {
            string report = new CommandCenter().ExecuteCommands(TestParsers.CreateDefault().Parse($"5 5\n2 2 {heading}"));

            Assert.Equal($"2 2 {heading}{Environment.NewLine}", report);
        }

        [Theory]
        [InlineData("n")]
        [InlineData("NE")]
        [InlineData("X")]
        public void Heading_ThatIsNotASingleCompassLetter_IsRejected(string heading)
        {
            Assert.Throws<ArgumentException>(() => TestParsers.CreateDefault().Parse($"5 5\n2 2 {heading}"));
        }
    }
}
