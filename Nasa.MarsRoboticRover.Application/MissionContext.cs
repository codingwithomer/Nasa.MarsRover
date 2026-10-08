using Nasa.MarsRoboticRover.Domain;
using System;
using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.Application
{
    /// <summary>The state of one mission run: the plateau and the rover currently being driven.</summary>
    public class MissionContext
    {
        private Plateau _plateau;
        private MarsRover _currentRover;

        public Plateau Plateau => _plateau ?? throw new InvalidOperationException("The plateau has not been defined yet.");

        public MarsRover CurrentRover => _currentRover ?? throw new InvalidOperationException("No rover has been deployed yet.");

        /// <summary>Every rover deployed so far, in deployment order; empty until a plateau exists.</summary>
        public IReadOnlyList<MarsRover> Rovers => _plateau?.Rovers ?? Array.Empty<MarsRover>();

        public void DefinePlateau(Plateau plateau)
        {
            ArgumentNullException.ThrowIfNull(plateau);

            if (_plateau != null)
            {
                throw new InvalidOperationException("The plateau has already been defined.");
            }

            _plateau = plateau;
        }

        public void DeployRover(Position position, CompassDirection compassDirection)
        {
            _currentRover = Plateau.Deploy(position, compassDirection);
        }
    }
}
