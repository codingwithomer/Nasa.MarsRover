using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Domain;

namespace Nasa.MarsRoboticRover.Application.Commands
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
