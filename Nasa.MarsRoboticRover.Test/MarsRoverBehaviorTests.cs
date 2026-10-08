using Nasa.MarsRoboticRover.Domain;
using Nasa.MarsRoboticRover.Domain.Interfaces;
using System;
using Xunit;

namespace Nasa.MarsRoboticRover.Test
{
    public class MarsRoverBehaviorTests
    {
        private static IRover CreateRover(int x, int y, CompassDirection direction)
        {
            return new Plateau(new Position(5, 5)).Deploy(new Position(x, y), direction);
        }

        [Theory]
        [InlineData(CompassDirection.North, 2, 3)]
        [InlineData(CompassDirection.East, 3, 2)]
        [InlineData(CompassDirection.South, 2, 1)]
        [InlineData(CompassDirection.West, 1, 2)]
        public void Move_AdvancesOneSquareInFacingDirection(CompassDirection direction, int expectedX, int expectedY)
        {
            IRover rover = CreateRover(2, 2, direction);

            rover.Move();

            Assert.Equal(new Position(expectedX, expectedY), rover.Position);
            Assert.Equal(direction, rover.CompassDirection);
        }

        [Theory]
        [InlineData(CompassDirection.North, CompassDirection.West)]
        [InlineData(CompassDirection.West, CompassDirection.South)]
        [InlineData(CompassDirection.South, CompassDirection.East)]
        [InlineData(CompassDirection.East, CompassDirection.North)]
        public void Rotate_Left_TurnsCounterClockwise(CompassDirection start, CompassDirection expected)
        {
            IRover rover = CreateRover(2, 2, start);

            rover.Rotate(Rotation.Left);

            Assert.Equal(expected, rover.CompassDirection);
            Assert.Equal(new Position(2, 2), rover.Position);
        }

        [Theory]
        [InlineData(CompassDirection.North, CompassDirection.East)]
        [InlineData(CompassDirection.East, CompassDirection.South)]
        [InlineData(CompassDirection.South, CompassDirection.West)]
        [InlineData(CompassDirection.West, CompassDirection.North)]
        public void Rotate_Right_TurnsClockwise(CompassDirection start, CompassDirection expected)
        {
            IRover rover = CreateRover(2, 2, start);

            rover.Rotate(Rotation.Right);

            Assert.Equal(expected, rover.CompassDirection);
            Assert.Equal(new Position(2, 2), rover.Position);
        }

        [Theory]
        [InlineData(0, 5, CompassDirection.North)]
        [InlineData(5, 0, CompassDirection.East)]
        [InlineData(0, 0, CompassDirection.South)]
        [InlineData(0, 0, CompassDirection.West)]
        public void Move_OffThePlateau_ThrowsAndKeepsPosition(int x, int y, CompassDirection direction)
        {
            IRover rover = CreateRover(x, y, direction);

            Assert.Throws<InvalidOperationException>(() => rover.Move());
            Assert.Equal(new Position(x, y), rover.Position);
        }
    }
}
