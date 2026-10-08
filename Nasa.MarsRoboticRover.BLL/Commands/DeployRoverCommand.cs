using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities;

namespace Nasa.MarsRoboticRover.BLL.Commands
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
            context.SetCurrentRover(context.Plateau.Deploy(_position, _compassDirection));
        }
    }
}
