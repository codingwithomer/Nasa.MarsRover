using Nasa.MarsRoboticRover.BLL.Interfaces;
using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.BLL.Parsing
{
    /// <summary>Turns one kind of input line into commands. Add a new implementation to support a new line kind.</summary>
    public interface ILineParser
    {
        LineKind Kind { get; }

        bool CanParse(InputLine line);

        IEnumerable<ICommand> Parse(InputLine line);
    }
}
