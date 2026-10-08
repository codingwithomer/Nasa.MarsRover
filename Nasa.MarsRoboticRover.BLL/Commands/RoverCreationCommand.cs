using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities;
using Nasa.MarsRoboticRover.Entities.Interfaces;

namespace Nasa.MarsRoboticRover.BLL.Commands
{
    public class RoverCreationCommand : ICommand
    {
        private readonly Position _position;
        private readonly CompassDirection _compassDirection;

        public RoverCreationCommand(Position position, CompassDirection compassDirection)
        {
            _position = position;
            _compassDirection = compassDirection;
        }

        public string Execute(ILocation location)
        {
            new MarsRover(_position, _compassDirection, location);
            return string.Empty;
        }
    }
}
