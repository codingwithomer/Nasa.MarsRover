using Nasa.MarsRoboticRover.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nasa.MarsRoboticRover.Entities
{
    public class Plateau : ILocation
    {
        private readonly List<IRover> _marsRover = new List<IRover>();
        private Position? _maxPosition;

        public void Initialize(Position position)
        {
            if (_maxPosition.HasValue)
            {
                throw new InvalidOperationException("Location is already initialized.");
            }

            _maxPosition = position;
        }

        public bool IsPositionValid(Position position)
        {
            return position.IsWithin(Position.Origin, GetMaxPosition());
        }

        public bool IsPositionFree(Position position)
        {
            EnsureInitialized();

            return !_marsRover.Any(r => r.Position == position);
        }

        public void AddRover(IRover marsRover)
        {
            EnsureInitialized();

            _marsRover.Add(marsRover);
        }

        public IRover GetRover()
        {
            EnsureInitialized();

            return _marsRover.Last();
        }

        private Position GetMaxPosition()
        {
            if (!_maxPosition.HasValue)
            {
                throw new InvalidOperationException("Location is not initialized.");
            }

            return _maxPosition.Value;
        }

        private void EnsureInitialized()
        {
            GetMaxPosition();
        }
    }
}
