namespace Nasa.MarsRoboticRover.Entities.Interfaces
{
    public interface ILocation
    {
        void Initialize(Position maxPosition);
        bool IsPositionValid(Position position);
        bool IsPositionFree(Position position);
        void AddRover(IRover marsRover);
        IRover GetRover();
    }
}
