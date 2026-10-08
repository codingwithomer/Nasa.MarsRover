using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Domain;

namespace Nasa.MarsRoboticRover.Application.Commands
{
    internal class DefinePlateauCommand : ICommand
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
