using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities;
using Nasa.MarsRoboticRover.Entities.Interfaces;

namespace Nasa.MarsRoboticRover.BLL.Commands
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
