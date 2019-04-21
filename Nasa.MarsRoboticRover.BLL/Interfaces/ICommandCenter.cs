using Nasa.MarsRoboticRover.Entities.Interfaces;
using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.BLL.Interfaces
{
    public interface ICommandCenter
    {
        string SetCommandInputs();
        void ExecuteCommands(List<ICommand> commands);
        string GetReportOutput();
    }
}
