using Nasa.MarsRoboticRover.Entities.Interfaces;

namespace Nasa.MarsRoboticRover.Entities
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
