namespace Nasa.MarsRoboticRover.Entities.Interfaces
{
    public interface IRover
    {
        Position Position { get; }
        CompassDirection CompassDirection { get; }

        void Rotate(Rotation rotation);
        void Move();
    }
}
