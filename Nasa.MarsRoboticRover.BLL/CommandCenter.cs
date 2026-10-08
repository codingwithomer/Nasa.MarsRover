using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace Nasa.MarsRoboticRover.BLL
{
    public class CommandCenter : ICommandCenter
    {
        private readonly ILocation _location;

        public CommandCenter(ILocation location)
        {
            _location = location;
        }

        public string ExecuteCommands(IEnumerable<ICommand> commands)
        {
            StringBuilder output = new StringBuilder();

            foreach (ICommand command in commands)
            {
                output.Append(command.Execute(_location));
            }

            return output.ToString();
        }
    }
}
