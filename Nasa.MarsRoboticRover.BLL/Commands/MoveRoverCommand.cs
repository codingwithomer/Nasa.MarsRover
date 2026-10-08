using Nasa.MarsRoboticRover.BLL.Interfaces;

namespace Nasa.MarsRoboticRover.BLL.Commands
{
    public class MoveRoverCommand : ICommand
    {
        public void Execute(MissionContext context)
        {
            context.CurrentRover.Move();
        }
    }
}
