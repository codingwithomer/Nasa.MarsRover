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
        public void Parse_TwoRovers_ProducesCommandsInExecutionOrder()
        {
            IReadOnlyList<ICommand> commands = _parser.Parse("5 5\n1 2 N\nLM\n3 3 E\nR");

            Type[] types = commands.Select(command => ((SourceLineCommand)command).Command.GetType()).ToArray();

            Assert.Equal(new[]
            {
                typeof(DefinePlateauCommand),
                typeof(DeployRoverCommand), typeof(RotateRoverCommand), typeof(MoveRoverCommand),
                typeof(DeployRoverCommand), typeof(RotateRoverCommand)
            }, types);
        }

        [Fact]
        public void Parse_PlateauOnly_ProducesOnlyThePlateauCommand()
        {
            IReadOnlyList<ICommand> commands = _parser.Parse("5 5");

            Assert.IsType<DefinePlateauCommand>(((SourceLineCommand)Assert.Single(commands)).Command);
        }

        [Fact]
        public void Parse_ToleratesBlankLinesExtraSpacesAndCrLf()
        {
            IReadOnlyList<ICommand> commands = _parser.Parse("  5   5 \r\n\r\n1  2   N\r\n  M  \r\n");

            Assert.Equal(3, commands.Count);
        }

        [Theory]
        [InlineData("5 5\nLM", "Instructions on line 2 must follow a rover position")]
        [InlineData("5 5\n1 2 N\n4 4\nM", "A rover position on line 3 needs X Y and a heading")]
        [InlineData("5 5\n1 2 N\nM\n1 2\nM", "A rover position on line 4 needs X Y and a heading")]
        public void Parse_BreakingTheInputOrder_ExplainsWhatIsWrong(string input, string expectedMessagePart)
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse(input));

            Assert.Contains(expectedMessagePart, ex.Message);
        }

        [Theory]
        [InlineData("-5 5")]
        [InlineData("a b")]
        [InlineData("+5 5")]
        [InlineData("5 5 5")]
        [InlineData("5")]
        [InlineData("LM")]
        [InlineData("1 2 N")]
        public void Parse_BadPlateauLine_SaysItMustBeTwoNonNegativeWholeNumbers(string plateau)
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse(plateau + "\n1 2 N\nM"));

            Assert.Contains("two non-negative whole numbers (X Y)", ex.Message);
            Assert.Contains("line 1", ex.Message);
            Assert.DoesNotContain("Unknown instruction", ex.Message);
        }

        [Theory]
        [InlineData("1 2")]
        [InlineData("1 2 N 4")]
        [InlineData("1")]
        public void Parse_RoverLineWithTheWrongShape_SaysARoverNeedsXYAndAHeading(string rover)
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse("5 5\n" + rover + "\nM"));

            Assert.Contains("A rover position on line 2 needs X Y and a heading", ex.Message);
        }

        [Theory]
        [InlineData("-1 2 N")]
        [InlineData("+1 2 N")]
        [InlineData("1a 2 N")]
        public void Parse_RoverLineWithABadCoordinate_IsAMalformedCoordinateLineNotAnInstruction(string rover)
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse("5 5\n" + rover + "\nM"));

            Assert.Contains("rover coordinates on line 2 must be non-negative whole numbers", ex.Message);
            Assert.DoesNotContain("Unknown instruction", ex.Message);
        }

        [Theory]
        [InlineData("n", "Letters must be upper case: use 'N' instead of 'n' as the heading on line 2")]
        [InlineData("X", "The heading on line 2 must be N, E, S or W, found 'X'")]
        [InlineData("NE", "The heading on line 2 must be N, E, S or W, found 'NE'")]
        public void Parse_BadHeading_ExplainsTheAllowedLetters(string heading, string expectedMessagePart)
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse("5 5\n1 2 " + heading + "\nM"));

            Assert.Contains(expectedMessagePart, ex.Message);
        }

        [Theory]
        [InlineData("l", "use 'L' instead of 'l'")]
        [InlineData("LmR", "use 'M' instead of 'm'")]
        public void Parse_LowercaseInstructions_AreRejectedWithAnUpperCaseHint(string instructions, string expectedMessagePart)
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse("5 5\n1 2 N\n" + instructions));

            Assert.Contains("Letters must be upper case", ex.Message);
            Assert.Contains(expectedMessagePart, ex.Message);
        }

        [Theory]
        [InlineData("99999999999 5\n1 1 N\nM")]
        [InlineData("5 5\n2147483648 1 N\nM")]
        [InlineData("5 5\n1 99999999999999999999 N\nM")]
        public void Parse_NumberBeyondIntRange_SaysTheNumberIsTooLarge(string input)
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse(input));

            Assert.Contains("is too large", ex.Message);
            Assert.DoesNotContain("Cannot parse", ex.Message);
        }

        [Fact]
        public void Parse_SpacesInsideInstructions_AreRejectedWithAClearMessage()
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse("5 5\n1 2 N\nM M"));

            Assert.Contains("Spaces are not allowed inside instructions on line 3", ex.Message);
        }

        [Theory]
        [InlineData("5 5\n1 2 N", "The rover on line 2 has no instructions")]
        [InlineData("5 5\n1 2 N\n3 3 E\nM", "The rover on line 2 has no instructions")]
        [InlineData("5 5\n1 2 N\nM\n3 3 E", "The rover on line 4 has no instructions")]
        public void Parse_RoverWithoutInstructions_IsRejected(string input, string expectedMessagePart)
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse(input));

            Assert.Contains(expectedMessagePart, ex.Message);
        }

        [Fact]
        public void Parse_AMissionWithOnlyAPlateau_IsAccepted()
        {
            Assert.Single(_parser.Parse("5 5"));
        }

        [Theory]
        [InlineData("5 5\r1 2 N\rM")]
        [InlineData("5 5\r\n1 2 N\r\nM")]
        [InlineData("\uFEFF5 5\n1 2 N\nM")]
        public void Parse_AcceptsLoneCarriageReturnsAndAByteOrderMark(string input)
        {
            Assert.Equal(3, _parser.Parse(input).Count);
        }

        [Fact]
        public void Parse_LoneCarriageReturns_KeepTheirLineNumbers()
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse("5 5\r1 2 N\rLX"));

            Assert.Contains("line 3", ex.Message);
        }

        [Fact]
        public void Parse_OnlyAByteOrderMark_IsEmptyInput()
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => _parser.Parse("\uFEFF"));

            Assert.StartsWith("Empty input.", ex.Message);
        }

        [Fact]
        public void Parse_SupportsNewInstructionLettersWithoutChangingTheParser()
        {
            InstructionSet instructions = InstructionSet.CreateDefault().With('B', new RotateRoverCommand(Rotation.Right));
            CommandParser parser = new CommandParser(new ILineParser[]
            {
                new PlateauLineParser(), new RoverLineParser(), new InstructionLineParser(instructions)
            });

            string report = new MissionExecutor().Execute(parser.Parse("5 5\n1 2 N\nB"));

            Assert.Equal("1 2 E" + Environment.NewLine, report);
        }

        [Fact]
        public void Parse_AcceptsTabsAsFieldSeparators()
        {
            IReadOnlyList<ICommand> commands = _parser.Parse("5\t5\n1\t2\tN\nM");

            Assert.Equal(3, commands.Count);
        }

        [Fact]
        public void With_ADuplicateLetter_ExplainsWhichLetterIsAlreadyDefined()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => InstructionSet.CreateDefault().With('L', new MoveRoverCommand()));

            Assert.Contains("letter 'L' is already defined", ex.Message);
        }

        [Fact]
        public void With_ANewLetter_LeavesTheOriginalSetUntouched()
        {
            InstructionSet original = InstructionSet.CreateDefault();

            InstructionSet extended = original.With('B', new MoveRoverCommand());

            Assert.False(original.TryGetCommand('B', out _));
            Assert.True(extended.TryGetCommand('B', out _));
        }
    }
}
