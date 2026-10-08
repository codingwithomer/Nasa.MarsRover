using Nasa.MarsRoboticRover.BLL.Commands;
using Nasa.MarsRoboticRover.BLL.Interfaces;
using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.BLL.Parsing
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
