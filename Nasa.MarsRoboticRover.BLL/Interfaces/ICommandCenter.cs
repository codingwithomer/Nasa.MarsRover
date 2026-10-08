using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.BLL.Interfaces
{
    public interface ICommandCenter
    {
        string ExecuteCommands(IEnumerable<ICommand> commands);
    }
}
