using Nasa.MarsRoboticRover.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nasa.MarsRoboticRover.Entities
{
    public class Plateau : ILocation
    {
        private Position _maxPosition;
        private List<IRover> _marsRover;

        public void Initialize(Position position)
        {
            if (_maxPosition != null)
            {
                throw new Exception("Location is already initialized.");
            }

            _maxPosition = position;
            _marsRover = new List<IRover>();
        }

        public bool IsPositionValid(Position position)
        {
            return position.IsWithin(Position.Origin, _maxPosition);
        }

        public bool IsPositionFree(Position position)
        {
            return !_marsRover.Any(r => r.Position == position);
        }

        public void AddRover(IRover marsRover)
        {
            _marsRover.Add(marsRover);
        }

        public IRover GetRover()
        {
            return _marsRover.Last();
        }
    }
}
