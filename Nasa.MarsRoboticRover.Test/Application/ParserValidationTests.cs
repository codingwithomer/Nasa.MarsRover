using Nasa.MarsRoboticRover.Application;
using System;
using Xunit;

namespace Nasa.MarsRoboticRover.Test.Application
{
    public class ParserValidationTests
    {
        private readonly CommandParser _parser = TestParsers.CreateDefault();

        [Fact]
        public void Parse_EmptyInput_ThrowsInvalidMission()
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse(string.Empty));

            Assert.StartsWith("Empty input.", ex.Message);
        }

        [Fact]
        public void Parse_NullInput_IsAProgrammingError()
        {
            Assert.Throws<ArgumentNullException>(() => _parser.Parse(null));
        }

        [Theory]
        [InlineData("5 5\n1 2 X\nM")]
        [InlineData("5 5\n1 2 N\nLMX")]
        [InlineData("5 5\n-1 2 N\nM")]
        [InlineData("5 5\n1 2 N 4\nM")]
        public void Parse_InvalidInput_ThrowsInvalidMissionWithoutDotNetParameterText(string input)
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse(input));

            Assert.DoesNotContain("Parameter", ex.Message);
        }

        [Theory]
        [InlineData("5 5\n1 2 N\nLMX", "line 3")]
        [InlineData("5 5\n\n\n1 2 N\nLMX", "line 5")]
        [InlineData("5 5\r\n1 2 X\r\nM", "line 2")]
        public void Parse_ErrorMessage_ReportsOneBasedLineNumberOfTheOriginalInput(string input, string expectedLine)
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse(input));

            Assert.Contains(expectedLine, ex.Message);
        }

        [Theory]
        [InlineData("5 5\n-1 2 N\nM", "Unknown instruction '-' on line 2")]
        [InlineData("5 5\n1 2 N\nLMX", "Unknown instruction 'X' on line 3")]
        [InlineData("5 5\n\u0663 2 N\nM", "Unknown instruction '\u0663' on line 2")]
        public void Parse_UnknownInstruction_NamesTheOffendingCharacterAndLine(string input, string expectedMessagePart)
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse(input));

            Assert.Contains(expectedMessagePart, ex.Message);
            Assert.Contains("Allowed instructions: L, M, R.", ex.Message);
        }
    }
}
