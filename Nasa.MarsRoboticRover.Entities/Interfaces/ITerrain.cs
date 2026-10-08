namespace Nasa.MarsRoboticRover.Entities.Interfaces
{
    /// <summary>What a rover needs to know about the ground it drives on.</summary>
    public interface ITerrain
    {
        bool IsPositionValid(Position position);
        bool IsPositionFree(Position position);
    }
}
