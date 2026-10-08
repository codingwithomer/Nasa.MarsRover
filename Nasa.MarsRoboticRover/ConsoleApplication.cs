using Microsoft.Extensions.DependencyInjection;
using Nasa.MarsRoboticRover.Application;
using Nasa.MarsRoboticRover.Application.Interfaces;
using System;
using System.IO;

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

        public static int Run(string[] args, TextReader input, bool inputRedirected, TextWriter output, TextWriter error)
        {
            if (args.Length > 1)
            {
                error.WriteLine("Usage: Nasa.MarsRoboticRover [mission-file]");
                return InvalidMission;
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
