using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.Application.Interfaces
{
    public interface IMissionExecutor
    {
        /// <summary>Runs one mission from scratch and returns its report: the final state of each rover, one line per rover.</summary>
        string Execute(IEnumerable<ICommand> commands);
    }
}
