using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.BLL.Interfaces
{
    public interface ICommandCenter
    {
        /// <summary>Runs one mission from scratch and returns its report, one line per rover.</summary>
        string ExecuteCommands(IEnumerable<ICommand> commands);
    }
}
