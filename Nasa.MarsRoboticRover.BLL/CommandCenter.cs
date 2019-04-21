using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nasa.MarsRoboticRover.BLL
{
    public class CommandCenter : ICommandCenter
    {
        private readonly StringBuilder _outputString;
        private readonly ILocation _location;

        public CommandCenter(ILocation location)
        {
            _location = location;

            _outputString = new StringBuilder();
            _outputString.AppendLine("Test Input:");
        }

        public string SetCommandInputs()
        {
            StringBuilder commandString = new StringBuilder();
            commandString.AppendLine("5 5");
            commandString.AppendLine("1 2 N");
            commandString.AppendLine("LMLMLMLMM");
            commandString.AppendLine("3 3 E");
            commandString.Append("MMRMMRMRRM");

            string commands = commandString.ToString();

            _outputString.AppendLine(commands);
            _outputString.AppendLine(Environment.NewLine);

            return commands;
        }

        public void ExecuteCommands(List<ICommand> commands)
        {
            _outputString.AppendLine("Expected Output:");

            foreach (ICommand command in commands)
            {
                string output = command.Execute(_location);

                if (!string.IsNullOrEmpty(output))
                {
                    _outputString.Append(output);
                }
            }

        }

        public string GetReportOutput()
        {
            return _outputString.ToString();
        }
    }
}
