using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nasa.MarsRoboticRover.Application
{
    public class MissionExecutor : IMissionExecutor
    {
        public string Execute(IEnumerable<ICommand> commands)
        {
            MissionContext context = new MissionContext();

            foreach (ICommand command in commands)
            {
                command.Execute(context);
            }

            return string.Concat(context.Rovers.Select(FormatReportLine));
        }

        private static string FormatReportLine(MarsRover rover)
        {
            return $"{rover.Position.X} {rover.Position.Y} {rover.CompassDirection.ToLetter()}{Environment.NewLine}";
        }
    }
}
