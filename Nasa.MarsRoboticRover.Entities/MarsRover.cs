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
                throw new ArgumentException("position", $"{position} is not valid.");

            if (!plateau.IsPositionFree(position))
                throw new ArgumentException("position", $"{position} is not free.");

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
            int x = Position.X;
            int y = Position.Y;

            switch (CompassDirection)
            {
                case CompassDirection.North:
                    y++;
                    break;
                case CompassDirection.East:
                    x++;
                    break;
                case CompassDirection.South:
                    y--;
                    break;
                case CompassDirection.West:
                    x--;
                    break;
            }

            Position position = new Position(x, y);

            if (!Plateau.IsPositionValid(position))
                throw new ArgumentException("position", $"{position} is not valid. Cannot move towards {CompassDirection} from current position {Position}.");

            if (!Plateau.IsPositionFree(position))
                throw new ArgumentException("position", $"{position} is not free. Cannot move towards {CompassDirection} from current position {Position}.");

            Position = position;
        }

        public string PrintPositionAndCompassDirection()
        {
            string compassDirectionStringValue = string.Empty;

            switch (CompassDirection)
            {
                case CompassDirection.North:
                    compassDirectionStringValue = "N";
                    break;
                case CompassDirection.East:
                    compassDirectionStringValue = "E";
                    break;
                case CompassDirection.South:
                    compassDirectionStringValue = "S";
                    break;
                case CompassDirection.West:
                    compassDirectionStringValue = "W";
                    break;
            }
            return $"{Position.X} {Position.Y} {compassDirectionStringValue}\r\n";
        }
    }
}