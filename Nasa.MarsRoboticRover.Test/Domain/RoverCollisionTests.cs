using Nasa.MarsRoboticRover.Domain;
using System;
using Xunit;

namespace Nasa.MarsRoboticRover.Test.Domain
{
    public class RoverCollisionTests
    {
        [Fact]
        public void Deploy_OnOccupiedPosition_Throws()
        {
            Plateau plateau = new Plateau(new Position(5, 5));
            plateau.Deploy(new Position(1, 1), CompassDirection.North);

            Assert.Throws<InvalidOperationException>(() => plateau.Deploy(new Position(1, 1), CompassDirection.East));
        }

        [Fact]
        public void Move_OntoOccupiedPosition_ThrowsAndKeepsPosition()
        {
            Plateau plateau = new Plateau(new Position(5, 5));
            plateau.Deploy(new Position(1, 1), CompassDirection.North);
            MarsRover mover = plateau.Deploy(new Position(1, 2), CompassDirection.South);

            Assert.Throws<InvalidOperationException>(() => mover.Move());
            Assert.Equal(new Position(1, 2), mover.Position);
        }
    }
}
