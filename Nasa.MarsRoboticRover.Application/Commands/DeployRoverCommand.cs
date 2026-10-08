using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Domain;

namespace Nasa.MarsRoboticRover.Application.Commands
{
    public class DeployRoverCommand : ICommand
    {
        private readonly Position _position;
        private readonly CompassDirection _compassDirection;

        public DeployRoverCommand(Position position, CompassDirection compassDirection)
        {
            _position = position;
            _compassDirection = compassDirection;
        }

        public void Execute(MissionContext context)
        {
            context.DeployRover(_position, _compassDirection);
        }
    }
}
