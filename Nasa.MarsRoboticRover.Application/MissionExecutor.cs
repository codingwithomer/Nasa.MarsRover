using Nasa.MarsRoboticRover.Application.Commands;
using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nasa.MarsRoboticRover.Application
{
    public class MissionExecutor : IMissionExecutor
    {
        public string Execute(IEnumerable<ICommand> commands)
        {
            ArgumentNullException.ThrowIfNull(commands);

            MissionContext context = new MissionContext();

            foreach (ICommand command in commands)
            {
                try
                {
                    command.Execute(context);
                }
                catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is InvalidOperationException)
                {
                    // A rover refused the mission. Rovers before the current one are finished and still reported.
                    throw new InvalidMissionException(Describe(command, context, ex), FormatFinishedRovers(context), ex);
                }
            }

            return FormatReport(context.Rovers);
        }

        private static string Describe(ICommand command, MissionContext context, Exception ex)
        {
            string problem = ex is ArgumentOutOfRangeException outOfRange
                ? DescribeOutOfRange(outOfRange)
                : ex.Message;

            string rover = context.RoverNumber == 0 ? string.Empty : $"Rover {context.RoverNumber}";
            string line = command is SourceLineCommand located ? $"line {located.Line}" : string.Empty;

            string prefix = (rover, line) switch
            {
                ("", "") => string.Empty,
                ("", _) => $"{char.ToUpperInvariant(line[0])}{line.Substring(1)}: ",
                (_, "") => $"{rover}: ",
                _ => $"{rover} ({line}): "
            };

            return prefix + problem;
        }

        // The message of an ArgumentOutOfRangeException carries .NET's "(Parameter ...)" text, so it is built here.
        private static string DescribeOutOfRange(ArgumentOutOfRangeException ex)
        {
            return ex.ActualValue is Position position
                ? $"{position} is outside the plateau."
                : "A value of the mission is out of range.";
        }

        private static string FormatFinishedRovers(MissionContext context)
        {
            // The rover being driven when the failure happened is the last one counted; it did not finish.
            return FormatReport(context.Rovers.Take(Math.Max(context.RoverNumber - 1, 0)));
        }

        private static string FormatReport(IEnumerable<MarsRover> rovers)
        {
            return string.Concat(rovers.Select(FormatReportLine));
        }

        private static string FormatReportLine(MarsRover rover)
        {
            return $"{rover.Position.X} {rover.Position.Y} {rover.CompassDirection.ToLetter()}{Environment.NewLine}";
        }
    }
}
