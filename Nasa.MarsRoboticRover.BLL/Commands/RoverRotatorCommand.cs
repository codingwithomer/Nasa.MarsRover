using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities;
using Nasa.MarsRoboticRover.Entities.Interfaces;

namespace Nasa.MarsRoboticRover.BLL.Commands
{
    public class RoverRotatorCommand : ICommand
    {
        private readonly Rotation _rotation;

        public RoverRotatorCommand(Rotation rotation)
        {
            _rotation = rotation;
        }

        public string Execute(ILocation location)
        {
            var rover = location.GetRover();
            rover.Rotate(_rotation);
            return string.Empty;
        }
    }
}
