using Nasa.MarsRoboticRover.Domain;
using Nasa.MarsRoboticRover.Domain.Interfaces;
using System;
using Xunit;

namespace Nasa.MarsRoboticRover.Test
{
    public class RoverCollisionTests
    {
        [Fact]
        public void Deploy_OnOccupiedPosition_Throws()
        {
            IPlateau plateau = new Plateau(new Position(5, 5));
            plateau.Deploy(new Position(1, 1), CompassDirection.North);

            Assert.Throws<ArgumentException>(() => plateau.Deploy(new Position(1, 1), CompassDirection.East));
        }

        [Fact]
        public void Move_OntoOccupiedPosition_ThrowsAndKeepsPosition()
        {
            IPlateau plateau = new Plateau(new Position(5, 5));
            plateau.Deploy(new Position(1, 1), CompassDirection.North);
            IRover mover = plateau.Deploy(new Position(1, 2), CompassDirection.South);

            Assert.Throws<InvalidOperationException>(() => mover.Move());
            Assert.Equal(new Position(1, 2), mover.Position);
        }
    }
}
