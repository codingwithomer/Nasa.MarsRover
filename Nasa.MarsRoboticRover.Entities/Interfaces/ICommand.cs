namespace Nasa.MarsRoboticRover.Entities.Interfaces
{
    public interface ICommand
    {
        string Execute(ILocation location);
    }
}
