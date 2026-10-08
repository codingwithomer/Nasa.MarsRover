using Nasa.MarsRoboticRover.Application.Commands;
using Nasa.MarsRoboticRover.Application.Interfaces;
using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.Application.Parsing
{
    /// <summary>"X Y H": where a rover lands and which way it faces.</summary>
    public class RoverLineParser : ILineParser
    {
        public LineKind Kind => LineKind.Rover;

        public bool CanParse(InputLine line)
        {
            return LineValueParser.StartsWithDigit(line) && line.Parts.Length == 3;
        }

        public IEnumerable<ICommand> Parse(InputLine line)
        {
            string[] parts = line.Parts;

            yield return new DeployRoverCommand(
                LineValueParser.ParsePosition(line, parts),
                LineValueParser.ParseCompassDirection(line, parts[2]));
        }
    }
}
