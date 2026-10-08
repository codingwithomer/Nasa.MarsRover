namespace Nasa.MarsRoboticRover.Entities.Interfaces
{
    public interface IPlateau : ITerrain
    {
        IRover Deploy(Position position, CompassDirection compassDirection);
    }
}
