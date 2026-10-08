using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities;

namespace Nasa.MarsRoboticRover.BLL.Commands
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
