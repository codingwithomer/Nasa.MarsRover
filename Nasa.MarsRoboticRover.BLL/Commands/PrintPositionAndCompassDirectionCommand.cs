using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities;

namespace Nasa.MarsRoboticRover.BLL.Commands
{
    public class PrintPositionAndCompassDirectionCommand : ICommand
    {
        public void Execute(MissionContext context)
        {
            Position position = context.CurrentRover.Position;
            char heading = context.CurrentRover.CompassDirection.ToLetter();

            context.AddReportLine($"{position.X} {position.Y} {heading}");
        }
    }
}
