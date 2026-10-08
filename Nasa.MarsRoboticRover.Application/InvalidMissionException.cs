using System;

namespace Nasa.MarsRoboticRover.Application
{
    /// <summary>
    /// The mission the user supplied is wrong: it cannot be parsed or a rover cannot carry it out.
    /// Anything else (null arguments, wiring problems, bugs) is a programming error and uses other exception types.
    /// </summary>
    public class InvalidMissionException : Exception
    {
        public InvalidMissionException(string message)
            : this(message, string.Empty, null)
        {
        }

        public InvalidMissionException(string message, string partialReport, Exception innerException)
            : base(message, innerException)
        {
            PartialReport = partialReport ?? string.Empty;
        }

        /// <summary>The report lines of the rovers that finished before the mission failed; empty if none did.</summary>
        public string PartialReport { get; }
    }
}
