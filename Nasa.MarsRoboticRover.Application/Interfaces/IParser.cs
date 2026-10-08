using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.Application.Interfaces
{
    public interface IParser
    {
        IReadOnlyList<ICommand> Parse(string commandInput);
    }
}
