using Microsoft.Extensions.DependencyInjection;
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

            using ServiceProvider serviceProvider = new ServiceCollection()
                .AddMarsRover()
                .AddSingleton(SelectInputProvider(args, input, inputRedirected))
                .BuildServiceProvider();

            try
            {
                output.WriteLine(serviceProvider.GetRequiredService<MissionRunner>().Run());
                return Success;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                error.WriteLine($"Invalid mission: {Describe(ex)}");
                return InvalidMission;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                error.WriteLine($"Cannot read the mission input: {ex.Message}");
                return InputUnreadable;
            }
        }

        /// <summary>The exception message without the "(Parameter 'x')" suffix .NET appends to argument exceptions.</summary>
        private static string Describe(Exception ex)
        {
            return ex is ArgumentException argumentException && argumentException.ParamName != null
                ? argumentException.Message.Replace($" (Parameter '{argumentException.ParamName}')", string.Empty)
                : ex.Message;
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
