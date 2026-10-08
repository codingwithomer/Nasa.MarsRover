using Nasa.MarsRoboticRover.Entities;
using System;
using Xunit;

namespace Nasa.MarsRoboticRover.Test
{
    public class PlateauTests
    {
        [Fact]
        public void IsPositionValid_BeforeInitialize_ThrowsInvalidOperation()
        {
            Plateau plateau = new Plateau();

            Assert.Throws<InvalidOperationException>(() => plateau.IsPositionValid(new Position(0, 0)));
        }

        [Fact]
        public void IsPositionFree_BeforeInitialize_ThrowsInvalidOperation()
        {
            Plateau plateau = new Plateau();

            Assert.Throws<InvalidOperationException>(() => plateau.IsPositionFree(new Position(0, 0)));
        }

        [Fact]
        public void Initialize_Twice_ThrowsInvalidOperation()
        {
            Plateau plateau = new Plateau();
            plateau.Initialize(new Position(5, 5));

            Assert.Throws<InvalidOperationException>(() => plateau.Initialize(new Position(3, 3)));
        }

        [Fact]
        public void GetRover_WithoutRovers_ThrowsInvalidOperation()
        {
            Plateau plateau = new Plateau();
            plateau.Initialize(new Position(5, 5));

            Assert.Throws<InvalidOperationException>(() => plateau.GetRover());
        }

        [Theory]
        [InlineData(0, 0, true)]
        [InlineData(5, 5, true)]
        [InlineData(-1, 0, false)]
        [InlineData(0, -1, false)]
        [InlineData(6, 5, false)]
        [InlineData(5, 6, false)]
        public void IsPositionValid_ChecksPlateauBounds(int x, int y, bool expected)
        {
            Plateau plateau = new Plateau();
            plateau.Initialize(new Position(5, 5));

            Assert.Equal(expected, plateau.IsPositionValid(new Position(x, y)));
        }
    }
}
