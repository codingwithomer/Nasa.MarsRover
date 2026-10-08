using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Domain;

namespace Nasa.MarsRoboticRover.Application.Commands
{
    internal class RotateRoverCommand : ICommand
    {
        private readonly Rotation _rotation;

        public RotateRoverCommand(Rotation rotation)
        {
            _rotation = rotation;
        }

        public void Execute(MissionContext context)
        {
            context.CurrentRover.Rotate(_rotation);
        }
    }
}
