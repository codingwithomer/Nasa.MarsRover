using Nasa.MarsRoboticRover.Application;
using Nasa.MarsRoboticRover.Application.Commands;
using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Application.Parsing;
using Nasa.MarsRoboticRover.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Nasa.MarsRoboticRover.Test.Application
{
    public class ParserBehaviorTests
    {
        private readonly CommandParser _parser = TestParsers.CreateDefault();

        [Fact]
        public void Parse_TwoRovers_ProducesCommandsInExecutionOrderWithOnePrintPerRover()
        {
            List<ICommand> commands = _parser.Parse("5 5\n1 2 N\nLM\n3 3 E\nR");

            Type[] types = commands.Select(command => command.GetType()).ToArray();

            Assert.Equal(new[]
            {
                typeof(DefinePlateauCommand),
                typeof(DeployRoverCommand), typeof(RoverRotatorCommand), typeof(MoveRoverCommand), typeof(PrintPositionAndCompassDirectionCommand),
                typeof(DeployRoverCommand), typeof(RoverRotatorCommand), typeof(PrintPositionAndCompassDirectionCommand)
            }, types);
        }

        [Fact]
        public void Parse_PlateauOnly_ProducesNoPrintCommand()
        {
            List<ICommand> commands = _parser.Parse("5 5");

            Assert.IsType<DefinePlateauCommand>(Assert.Single(commands));
        }

        [Fact]
        public void Parse_ToleratesBlankLinesExtraSpacesAndCrLf()
        {
            List<ICommand> commands = _parser.Parse("  5   5 \r\n\r\n1  2   N\r\n  M  \r\n");

            Assert.Equal(4, commands.Count);
        }

        [Theory]
        [InlineData("1 2 N\nM", "first line must define the plateau")]
        [InlineData("5 5\nLM", "must follow a rover position")]
        [InlineData("5 5\n1 2 N\n4 4", "first line on line 3")]
        [InlineData("5 5\n1 2\nM", "first line on line 2")]
        [InlineData("5 5\n1 2 N 4\nM", "Line 2 is not a plateau")]
        [InlineData("5 5\n1a 2 N\nM", "Cannot parse positive integer from 1a on line 2")]
        public void Parse_BreakingTheInputOrder_ExplainsWhatIsWrong(string input, string expectedMessagePart)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _parser.Parse(input));

            Assert.Contains(expectedMessagePart, ex.Message);
        }

        [Fact]
        public void Parse_SupportsNewInstructionLettersWithoutChangingTheParser()
        {
            Dictionary<char, Func<ICommand>> instructions = new Dictionary<char, Func<ICommand>>(InstructionLineParser.DefaultInstructions)
            {
                ['B'] = () => new RoverRotatorCommand(Rotation.Right)
            };
            CommandParser parser = new CommandParser(new ILineParser[]
            {
                new PlateauLineParser(), new RoverLineParser(), new InstructionLineParser(instructions)
            });

            ICommandCenter commandCenter = new CommandCenter();
            string report = commandCenter.ExecuteCommands(parser.Parse("5 5\n1 2 N\nB"));

            Assert.Equal("1 2 E" + Environment.NewLine, report);
        }
    }
}
