using Nasa.MarsRoboticRover.Entities.Interfaces;

namespace Nasa.MarsRoboticRover.Entities.Commands
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
