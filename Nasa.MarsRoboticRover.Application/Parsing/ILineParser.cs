using Nasa.MarsRoboticRover.Application.Interfaces;
using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.Application.Parsing
{
    /// <summary>Turns one kind of input line into commands. Add a new implementation to support a new line kind.</summary>
    public interface ILineParser
    {
        LineKind Kind { get; }

        bool CanParse(InputLine line);

        /// <summary>Parses eagerly: content errors surface here, before the coordinator checks the line order.</summary>
        IReadOnlyList<ICommand> Parse(InputLine line);
    }
}
