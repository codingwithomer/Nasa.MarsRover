using Nasa.MarsRoboticRover.Entities.Interfaces;
using System;

namespace Nasa.MarsRoboticRover.Entities
{
    public class MarsRover : IRover
    {
        private const int DirectionCount = 4;

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
            int turned = (int)CompassDirection + (int)rotation + DirectionCount;
            CompassDirection = (CompassDirection)(turned % DirectionCount);
        }

        public void Move()
        {
            Position target = CompassDirection switch
            {
                CompassDirection.North => new Position(Position.X, Position.Y + 1),
                CompassDirection.East => new Position(Position.X + 1, Position.Y),
                CompassDirection.South => new Position(Position.X, Position.Y - 1),
                CompassDirection.West => new Position(Position.X - 1, Position.Y),
                _ => throw new InvalidOperationException($"Unknown compass direction {CompassDirection}.")
            };

            if (!_terrain.IsPositionValid(target))
            {
                throw new InvalidOperationException($"{target} is not valid. Cannot move towards {CompassDirection} from current position {Position}.");
            }

            if (!_terrain.IsPositionFree(target))
            {
                throw new InvalidOperationException($"{target} is not free. Cannot move towards {CompassDirection} from current position {Position}.");
            }

            Position = target;
        }
    }
}
