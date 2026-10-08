using Nasa.MarsRoboticRover.Domain;
using System;
using System.Collections.Generic;

namespace Nasa.MarsRoboticRover.Application
{
    /// <summary>The state of one mission run: the plateau, the rover being driven and the report so far.</summary>
    public class MissionContext
    {
        private readonly List<string> _reportLines = new List<string>();
        private Plateau _plateau;
        private MarsRover _currentRover;

        public Plateau Plateau => _plateau ?? throw new InvalidOperationException("The plateau has not been defined yet.");

        public MarsRover CurrentRover => _currentRover ?? throw new InvalidOperationException("No rover has been deployed yet.");

        public IReadOnlyList<string> ReportLines => _reportLines;

        public void DefinePlateau(Plateau plateau)
        {
            if (_plateau != null)
            {
                throw new InvalidOperationException("The plateau has already been defined.");
            }

            _plateau = plateau;
        }

        public void SetCurrentRover(MarsRover rover)
        {
            _currentRover = rover;
        }

        public void AddReportLine(string line)
        {
            _reportLines.Add(line);
        }
    }
}
