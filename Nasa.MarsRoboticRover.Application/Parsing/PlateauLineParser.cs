using Nasa.MarsRoboticRover.Application.Commands;
using Nasa.MarsRoboticRover.Application.Interfaces;
using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.Application.Parsing
{
    /// <summary>"X Y": the upper-right corner of the plateau.</summary>
    public class PlateauLineParser : ILineParser
    {
        public LineKind Kind => LineKind.Plateau;

        public bool CanParse(InputLine line)
        {
            return LineValueParser.StartsWithDigit(line) && line.Parts.Length == 2;
        }

        public IEnumerable<ICommand> Parse(InputLine line)
        {
            yield return new DefinePlateauCommand(LineValueParser.ParsePosition(line, line.Parts));
        }
    }
}
