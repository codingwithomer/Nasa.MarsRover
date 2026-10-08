using Nasa.MarsRoboticRover.Application.Interfaces;

namespace Nasa.MarsRoboticRover.Application.Commands
{
    internal class MoveRoverCommand : ICommand
    {
        public void Execute(MissionContext context)
        {
            context.CurrentRover.Move();
        }
    }
}
