using Nasa.MarsRoboticRover.Entities;
using Xunit;
using System;

namespace Nasa.MarsRoboticRover.Test
{
    public class RoverCollisionTests
    {
        private static Plateau CreatePlateau(int maxX = 5, int maxY = 5)
        {
            Plateau plateau = new Plateau();
            plateau.Initialize(new Position(maxX, maxY));
            return plateau;
        }

        [Fact]
        public void Rover_ShouldNotBeCreated_OnOccupiedPosition()
        {
            Plateau plateau = CreatePlateau();
            new MarsRover(new Position(1, 1), CompassDirection.North, plateau);

            Assert.Throws<ArgumentException>(() =>
                new MarsRover(new Position(1, 1), CompassDirection.East, plateau));
        }

        [Fact]
        public void Rover_ShouldNotMove_OntoOccupiedPosition()
        {
            Plateau plateau = CreatePlateau();
            new MarsRover(new Position(1, 1), CompassDirection.North, plateau);
            MarsRover mover = new MarsRover(new Position(1, 2), CompassDirection.South, plateau);

            Assert.Throws<ArgumentException>(() => mover.Move());
            Assert.Equal(new Position(1, 2), mover.Position);
        }
    }
}
