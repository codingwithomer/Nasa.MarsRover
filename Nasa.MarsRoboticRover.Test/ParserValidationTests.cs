using Nasa.MarsRoboticRover.BLL;
using System;
using Xunit;

namespace Nasa.MarsRoboticRover.Test
{
    public class ParserValidationTests
    {
        private readonly CommandParser _parser = new CommandParser();

        [Fact]
        public void Parse_EmptyInput_ThrowsWithInputAsParamName()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _parser.Parse(string.Empty));

            Assert.Equal("input", ex.ParamName);
            Assert.StartsWith("Empty input.", ex.Message);
        }

        [Theory]
        [InlineData("5 5\n1 2 X\nM")]
        [InlineData("5 5\n1 2 N\nLMX")]
        [InlineData("5 5\n-1 2 N\nM")]
        [InlineData("5 5\n1 2 N 4\nM")]
        public void Parse_InvalidInput_ThrowsArgumentExceptionWithInputAsParamName(string input)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _parser.Parse(input));

            Assert.Equal("input", ex.ParamName);
        }

        [Theory]
        [InlineData("5 5\n1 2 N\nLMX", "line 3")]
        [InlineData("5 5\n\n\n1 2 N\nLMX", "line 5")]
        [InlineData("5 5\r\n1 2 X\r\nM", "line 2")]
        public void Parse_ErrorMessage_ReportsOneBasedLineNumberOfTheOriginalInput(string input, string expectedLine)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _parser.Parse(input));

            Assert.Contains(expectedLine, ex.Message);
        }
    }
}
