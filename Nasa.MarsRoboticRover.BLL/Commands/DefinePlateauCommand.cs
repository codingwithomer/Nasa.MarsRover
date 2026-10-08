using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities;

namespace Nasa.MarsRoboticRover.BLL.Commands
{
    public class DefinePlateauCommand : ICommand
    {
        private readonly Position _upperRight;

        public DefinePlateauCommand(Position upperRight)
        {
            _upperRight = upperRight;
        }

        public void Execute(MissionContext context)
        {
            context.DefinePlateau(new Plateau(_upperRight));
        }
    }
}
