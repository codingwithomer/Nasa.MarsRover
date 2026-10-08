using Xunit;
using System;

namespace Nasa.MarsRoboticRover.Test.Presentation
{
    public class MissionReportFormatterTests
    {
        [Fact]
        public void Format_WrapsInputAndResultsWithHeaders()
        {
            string output = MissionReportFormatter.Format("5 5", "1 3 N" + Environment.NewLine);

            string expected = string.Join(Environment.NewLine, "Test Input:", "5 5", "", "", "Expected Output:", "1 3 N", "");
            Assert.Equal(expected, output);
        }
    }
}
