using Nasa.MarsRoboticRover.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nasa.MarsRoboticRover.Domain
{
    public class Plateau : ITerrain
    {
        private readonly List<MarsRover> _rovers = new List<MarsRover>();
        private readonly IReadOnlyList<MarsRover> _readOnlyRovers;
        private readonly Position _upperRight;

        public Plateau(Position upperRight)
        {
            if (upperRight.X < 0 || upperRight.Y < 0)
            {
                throw new ArgumentException($"{upperRight} cannot have negative coordinates.", nameof(upperRight));
            }

            _upperRight = upperRight;
            _readOnlyRovers = _rovers.AsReadOnly();
        }

        /// <summary>The deployed rovers in deployment order; a read-only view that follows later deployments.</summary>
        public IReadOnlyList<MarsRover> Rovers => _readOnlyRovers;

        public bool IsPositionValid(Position position)
        {
            return position.IsWithin(Position.Origin, _upperRight);
        }

        public bool IsPositionFree(Position position)
        {
            return !_rovers.Any(rover => rover.Position == position);
        }

        public MarsRover Deploy(Position position, CompassDirection compassDirection)
        {
            if (!Enum.IsDefined(compassDirection))
            {
                throw new ArgumentOutOfRangeException(nameof(compassDirection), compassDirection, "Unknown compass direction.");
            }

            if (!IsPositionValid(position))
            {
                throw new ArgumentOutOfRangeException(nameof(position), position, $"{position} is outside the plateau.");
            }

            if (!IsPositionFree(position))
            {
                throw new InvalidOperationException($"{position} is already occupied by another rover.");
            }

            MarsRover rover = new MarsRover(position, compassDirection, this);
            _rovers.Add(rover);

            return rover;
        }
    }
}
