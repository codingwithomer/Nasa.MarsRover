using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Domain;

namespace Nasa.MarsRoboticRover.Application.Commands
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
