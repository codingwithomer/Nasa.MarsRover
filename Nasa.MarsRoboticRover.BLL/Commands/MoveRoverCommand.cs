using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities;
using Nasa.MarsRoboticRover.Entities.Interfaces;

namespace Nasa.MarsRoboticRover.BLL.Commands
{
    public class MoveRoverCommand : ICommand
    {
        public string Execute(ILocation location)
        {
            IRover rover = location.GetRover();
            rover.Move();
            return string.Empty;
        }
    }
}
