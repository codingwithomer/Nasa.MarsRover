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
            return !line.IsFirst && LineValueParser.LooksLikeCoordinates(line);
        }

        public IReadOnlyList<ICommand> Parse(InputLine line)
        {
            string[] parts = line.Parts;

            return new ICommand[]
            {
                new DeployRoverCommand(
                    LineValueParser.ParseRoverPosition(line, parts),
                    LineValueParser.ParseCompassDirection(line, parts[2]))
            };
        }
    }
}
