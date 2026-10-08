using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Domain;

namespace Nasa.MarsRoboticRover.Application.Commands
{
    public class RoverRotatorCommand : ICommand
    {
        private readonly Rotation _rotation;

        public RoverRotatorCommand(Rotation rotation)
        {
            _rotation = rotation;
        }

        public void Execute(MissionContext context)
        {
            context.CurrentRover.Rotate(_rotation);
        }
    }
}
