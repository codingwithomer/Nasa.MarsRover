using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities;
using Nasa.MarsRoboticRover.Entities.Interfaces;

namespace Nasa.MarsRoboticRover.BLL.Commands
{
    public class LocationInitializeCommand : ICommand
    {
        private readonly Position _position;

        public LocationInitializeCommand(Position position)
        {
            _position = position;
        }

        public string Execute(ILocation location)
        {
            location.Initialize(_position);
            return string.Empty;
        }
    }
}
