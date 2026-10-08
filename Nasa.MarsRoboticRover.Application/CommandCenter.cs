using Nasa.MarsRoboticRover.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nasa.MarsRoboticRover.Application
{
    public class CommandCenter : ICommandCenter
    {
        public string ExecuteCommands(IEnumerable<ICommand> commands)
        {
            MissionContext context = new MissionContext();

            foreach (ICommand command in commands)
            {
                command.Execute(context);
            }

            return string.Concat(context.ReportLines.Select(line => line + Environment.NewLine));
        }
    }
}
