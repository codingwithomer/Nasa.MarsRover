using Nasa.MarsRoboticRover.Entities.Interfaces;

namespace Nasa.MarsRoboticRover.Entities.Commands
{
    public class PrintPositionAndCompassDirectionCommand : ICommand
    {
        public string Execute(ILocation location)
        {
            var rover = location.GetRover();
            return rover.PrintPositionAndCompassDirection();
        }
    }
}
