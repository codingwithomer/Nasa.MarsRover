namespace Nasa.MarsRoboticRover.Entities.Interfaces
{
    public interface IRover
    {
        Position Position { get; }

        void Rotate(Rotation rotation);
        void Move();
        string PrintPositionAndCompassDirection();
    }
}
