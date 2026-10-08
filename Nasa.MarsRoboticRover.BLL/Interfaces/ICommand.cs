using Nasa.MarsRoboticRover.Entities.Interfaces;

namespace Nasa.MarsRoboticRover.BLL.Interfaces
{
    public interface ICommand
    {
        string Execute(ILocation location);
    }
}
