using Microsoft.Extensions.DependencyInjection;
using Nasa.MarsRoboticRover.Application;
using Nasa.MarsRoboticRover.Application.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace Nasa.MarsRoboticRover
{
    /// <summary>
    /// The command line behind Main, with its streams passed in so it can be tested.
    /// Input comes from the file named by the single argument, else from standard input when it is
    /// redirected, else from the built-in sample mission.
    /// </summary>
    public static class ConsoleApplication
    {
        public const int Success = 0;
        public const int InvalidMission = 1;
        public const int InputUnreadable = 2;

        private const string UsageLine = "Usage: Nasa.MarsRoboticRover [mission-file | --help]";

        private static readonly string HelpText = string.Join(Environment.NewLine,
            UsageLine,
            "",
            "Drives rovers over a plateau and prints the final position of each one (X Y H, one line per rover).",
            "",
            "  mission-file   path of a text file that contains the mission",
            "  (no argument)  the mission is read from standard input when it is redirected;",
            "                 otherwise the built-in sample mission runs and is shown with its input",
            "  -h, --help     show this text",
            "",
            "Exit codes: 0 success, 1 invalid mission or usage, 2 the mission input could not be read",
            "");

        public static int Run(string[] args, TextReader input, bool inputRedirected, TextWriter output, TextWriter error)
        {
            if (args.Any(argument => argument == "--help" || argument == "-h"))
            {
                output.Write(HelpText);
                return Success;
            }

            if (args.Length > 1)
            {
                error.WriteLine(UsageLine);
                return InvalidMission;
            }

            if (args.Length == 1 && DescribeUnusablePath(args[0]) is string problem)
            {
                error.WriteLine($"Cannot read the mission input: {problem}");
                return InputUnreadable;
            }

            IMissionInputProvider inputProvider = SelectInputProvider(args, input, inputRedirected);

            using ServiceProvider serviceProvider = new ServiceCollection()
                .AddMarsRover()
                .AddSingleton(inputProvider)
                .BuildServiceProvider();

            MissionRunner runner = serviceProvider.GetRequiredService<MissionRunner>();

            try
            {
                string report = runner.Run();

                if (inputProvider is SampleMissionInputProvider)
                {
                    // Only the built-in sample is presented as an example; real missions print just the report.
                    output.WriteLine(MissionReportFormatter.Format(inputProvider.GetInput(), report));
                }
                else
                {
                    output.Write(report);
                }

                return Success;
            }
            catch (InvalidMissionException ex)
            {
                // Rovers that finished before the failure are still reported.
                output.Write(ex.PartialReport);
                error.WriteLine($"Invalid mission: {ex.Message}");
                return InvalidMission;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException)
            {
                error.WriteLine($"Cannot read the mission input: {ex.Message}");
                return InputUnreadable;
            }
        }

        /// <summary>Only an interactive run of the built-in sample waits for a key, so that a console window stays open.</summary>
        public static bool ShouldWaitForKey(string[] args, bool inputRedirected, int exitCode)
        {
            return args.Length == 0 && !inputRedirected && exitCode == Success;
        }

        private static string DescribeUnusablePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return "The mission file name is empty.";
            }

            return Directory.Exists(path) ? $"'{path}' is a directory, not a mission file." : null;
        }

        private static IMissionInputProvider SelectInputProvider(string[] args, TextReader input, bool inputRedirected)
        {
            if (args.Length == 1)
            {
                return new FileMissionInputProvider(args[0]);
            }

            if (inputRedirected)
            {
                return new TextReaderMissionInputProvider(input);
            }

            return new SampleMissionInputProvider();
        }
    }
}
