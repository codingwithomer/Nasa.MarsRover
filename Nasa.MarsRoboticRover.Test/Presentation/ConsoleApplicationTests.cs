using System;
using System.IO;
using Xunit;

namespace Nasa.MarsRoboticRover.Test.Presentation
{
    public class ConsoleApplicationTests
    {
        private readonly StringWriter _output = new StringWriter();
        private readonly StringWriter _error = new StringWriter();

        private int Run(string[] args, string stdin = "", bool inputRedirected = false)
        {
            return ConsoleApplication.Run(args, new StringReader(stdin), inputRedirected, _output, _error);
        }

        [Fact]
        public void Run_WithoutArgumentsAndInteractiveInput_RunsTheDecoratedSampleMission()
        {
            int exitCode = Run(Array.Empty<string>());

            string expected = string.Join(Environment.NewLine,
                "Test Input:", "5 5", "1 2 N", "LMLMLMLMM", "3 3 E", "MMRMMRMRRM", "", "",
                "Expected Output:", "1 3 N", "5 1 E", "", "");
            Assert.Equal(ConsoleApplication.Success, exitCode);
            Assert.Equal(expected, _output.ToString());
            Assert.Equal(string.Empty, _error.ToString());
        }

        [Fact]
        public void Run_WithRedirectedInput_PrintsOnlyTheReport()
        {
            int exitCode = Run(Array.Empty<string>(), "3 3\n0 0 N\nMMR\n", inputRedirected: true);

            Assert.Equal(ConsoleApplication.Success, exitCode);
            Assert.Equal("0 2 E" + Environment.NewLine, _output.ToString());
        }

        [Fact]
        public void Run_WithRedirectedInputStartingWithAByteOrderMark_ReadsTheMission()
        {
            int exitCode = Run(Array.Empty<string>(), "\uFEFF3 3\n0 0 N\nMMR\n", inputRedirected: true);

            Assert.Equal(ConsoleApplication.Success, exitCode);
            Assert.Equal("0 2 E" + Environment.NewLine, _output.ToString());
        }

        [Fact]
        public void Run_WithEmptyRedirectedInput_FailsInsteadOfRunningTheSample()
        {
            int exitCode = Run(Array.Empty<string>(), string.Empty, inputRedirected: true);

            Assert.Equal(ConsoleApplication.InvalidMission, exitCode);
            Assert.Equal("Invalid mission: Empty input." + Environment.NewLine, _error.ToString());
            Assert.Equal(string.Empty, _output.ToString());
        }

        [Fact]
        public void Run_WithAPlateauOnlyMission_SucceedsWithAnEmptyReport()
        {
            int exitCode = Run(Array.Empty<string>(), "5 5\n", inputRedirected: true);

            Assert.Equal(ConsoleApplication.Success, exitCode);
            Assert.Equal(string.Empty, _output.ToString());
        }

        [Fact]
        public void Run_WithAFilePath_PrintsOnlyTheReportOfThatFile()
        {
            string path = Path.GetTempFileName();
            try
            {
                File.WriteAllText(path, "4 4\n1 1 E\nMM\n");

                int exitCode = Run(new[] { path });

                Assert.Equal(ConsoleApplication.Success, exitCode);
                Assert.Equal("3 1 E" + Environment.NewLine, _output.ToString());
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void Run_WithAnInvalidMission_ExplainsOnStandardErrorAndReturnsOne()
        {
            int exitCode = Run(Array.Empty<string>(), "5 5\n6 6 N\n", inputRedirected: true);

            Assert.Equal(ConsoleApplication.InvalidMission, exitCode);
            Assert.StartsWith("Invalid mission:", _error.ToString());
            Assert.Equal(string.Empty, _output.ToString());
        }

        [Fact]
        public void Run_WhenARoverFails_PrintsTheFinishedRoversAndNamesTheFailedOneOnStandardError()
        {
            int exitCode = Run(Array.Empty<string>(), "5 5\n1 1 N\nM\n3 3 E\nM\n5 5 N\nM\n", inputRedirected: true);

            Assert.Equal(ConsoleApplication.InvalidMission, exitCode);
            Assert.Equal(string.Join(Environment.NewLine, "1 2 N", "4 3 E", ""), _output.ToString());
            Assert.Contains("Rover 3 (line 7)", _error.ToString());
        }

        [Fact]
        public void Run_WithARoverDeployedOutsideThePlateau_NeverShowsDotNetArgumentText()
        {
            Run(Array.Empty<string>(), "5 5\n9 9 N\nM\n", inputRedirected: true);

            Assert.Equal("Invalid mission: Rover 1 (line 2): (9, 9) is outside the plateau." + Environment.NewLine, _error.ToString());
        }

        [Fact]
        public void Run_WithAMalformedLine_ReportsTheLineNumber()
        {
            int exitCode = Run(Array.Empty<string>(), "5 5\n1 2 N\nLMX\n", inputRedirected: true);

            Assert.Equal(ConsoleApplication.InvalidMission, exitCode);
            Assert.Contains("line 3", _error.ToString());
            Assert.DoesNotContain("(Parameter", _error.ToString());
        }

        [Fact]
        public void Run_WithAMissingFile_ReturnsTwo()
        {
            int exitCode = Run(new[] { Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt") });

            Assert.Equal(ConsoleApplication.InputUnreadable, exitCode);
            Assert.StartsWith("Cannot read the mission input:", _error.ToString());
        }

        [Fact]
        public void Run_WithTooManyArguments_PrintsUsage()
        {
            int exitCode = Run(new[] { "a.txt", "b.txt" });

            Assert.Equal(ConsoleApplication.InvalidMission, exitCode);
            Assert.StartsWith("Usage:", _error.ToString());
        }
    }
}
