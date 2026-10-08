using Nasa.MarsRoboticRover.Domain.Interfaces;
using System;

namespace Nasa.MarsRoboticRover.Domain
{
    public class MarsRover
    {
        private readonly ITerrain _terrain;

        public Position Position { get; private set; }
        public CompassDirection CompassDirection { get; private set; }

        internal MarsRover(Position position, CompassDirection compassDirection, ITerrain terrain)
        {
            Position = position;
            CompassDirection = compassDirection;
            _terrain = terrain;
        }

        public void Rotate(Rotation rotation)
        {
            CompassDirection = rotation switch
            {
                Rotation.Left => CompassDirection.TurnLeft(),
                Rotation.Right => CompassDirection.TurnRight(),
                _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
            };
        }

        public void Move()
        {
            (int dx, int dy) = CompassDirection switch
            {
                CompassDirection.North => (0, 1),
                CompassDirection.East => (1, 0),
                CompassDirection.South => (0, -1),
                CompassDirection.West => (-1, 0),
                _ => throw new InvalidOperationException($"Unknown compass direction {CompassDirection}.")
            };

            // long arithmetic: a coordinate next to int.MaxValue must not wrap around to a "valid" square.
            long x = (long)Position.X + dx;
            long y = (long)Position.Y + dy;

            if (x != (int)x || y != (int)y)
            {
                throw new InvalidOperationException($"The next square is outside the plateau; it cannot move {CompassDirection} from {Position}.");
            }

            Position target = new Position((int)x, (int)y);

            if (!_terrain.IsPositionValid(target))
            {
                throw new InvalidOperationException($"{target} is outside the plateau; it cannot move {CompassDirection} from {Position}.");
            }

            if (!_terrain.IsPositionFree(target))
            {
                throw new InvalidOperationException($"{target} is occupied by another rover; it cannot move {CompassDirection} from {Position}.");
            }

            Position = target;
        }
    }
}
