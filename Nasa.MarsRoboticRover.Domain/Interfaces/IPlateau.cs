namespace Nasa.MarsRoboticRover.Domain.Interfaces
{
    public interface IPlateau : ITerrain
    {
        IRover Deploy(Position position, CompassDirection compassDirection);
    }
}
