using Nasa.MarsRoboticRover.Application.Commands;
using Nasa.MarsRoboticRover.Application.Interfaces;
using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.Application.Parsing
{
    /// <summary>"X Y": the upper-right corner of the plateau. It is always the first line.</summary>
    public class PlateauLineParser : ILineParser
    {
        public LineKind Kind => LineKind.Plateau;

        public bool CanParse(InputLine line)
        {
            return line.IsFirst;
        }

        public IReadOnlyList<ICommand> Parse(InputLine line)
        {
            return new ICommand[] { new DefinePlateauCommand(LineValueParser.ParsePlateau(line)) };
        }
    }
}
