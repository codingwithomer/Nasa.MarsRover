using Nasa.MarsRoboticRover.Entities.Interfaces;
using System;

namespace Nasa.MarsRoboticRover.Entities
{
    public class MarsRover : IRover
    {
        public Position Position { get; private set; }
        public CompassDirection CompassDirection { get; private set; }
        public ILocation Plateau { get; private set; }

        public MarsRover(Position position, CompassDirection compassDirection, ILocation plateau)
        {
            if (!plateau.IsPositionValid(position))
                throw new ArgumentException($"{position} is not valid.", "position");

            if (!plateau.IsPositionFree(position))
                throw new ArgumentException($"{position} is not free.", "position");

            Position = position;
            CompassDirection = compassDirection;
            Plateau = plateau;
            Plateau.AddRover(this);
        }

        public void Rotate(Rotation rotation)
        {
            int compassDirectionIntValue = (int)CompassDirection + (int)rotation + 4;
            CompassDirection = (CompassDirection)(compassDirectionIntValue % 4);
        }

        public void Move()
        {
            Position position = CompassDirection switch
            {
                CompassDirection.North => new Position(Position.X, Position.Y + 1),
                CompassDirection.East => new Position(Position.X + 1, Position.Y),
                CompassDirection.South => new Position(Position.X, Position.Y - 1),
                CompassDirection.West => new Position(Position.X - 1, Position.Y),
                _ => throw new InvalidOperationException($"Unknown compass direction {CompassDirection}.")
            };

            if (!Plateau.IsPositionValid(position))
                throw new ArgumentException($"{position} is not valid. Cannot move towards {CompassDirection} from current position {Position}.", "position");

            if (!Plateau.IsPositionFree(position))
                throw new ArgumentException($"{position} is not free. Cannot move towards {CompassDirection} from current position {Position}.", "position");

            Position = position;
        }

        public string PrintPositionAndCompassDirection()
        {
            return $"{Position.X} {Position.Y} {CompassDirection.ToLetter()}{Environment.NewLine}";
        }
    }
}
