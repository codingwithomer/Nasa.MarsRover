using Nasa.MarsRoboticRover.Domain;
using Nasa.MarsRoboticRover.Domain.Interfaces;
using System;
using Xunit;

namespace Nasa.MarsRoboticRover.Test
{
    public class PlateauTests
    {
        [Theory]
        [InlineData(-1, 5)]
        [InlineData(5, -1)]
        public void Constructor_NegativeUpperRight_Throws(int x, int y)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => new Plateau(new Position(x, y)));

            Assert.Equal("upperRight", ex.ParamName);
        }

        [Theory]
        [InlineData(0, 0, true)]
        [InlineData(5, 5, true)]
        [InlineData(-1, 0, false)]
        [InlineData(0, -1, false)]
        [InlineData(6, 5, false)]
        [InlineData(5, 6, false)]
        public void IsPositionValid_ChecksInclusiveBounds(int x, int y, bool expected)
        {
            ITerrain plateau = new Plateau(new Position(5, 5));

            Assert.Equal(expected, plateau.IsPositionValid(new Position(x, y)));
        }

        [Fact]
        public void Deploy_OutsideThePlateau_Throws()
        {
            IPlateau plateau = new Plateau(new Position(5, 5));

            ArgumentException ex = Assert.Throws<ArgumentException>(() => plateau.Deploy(new Position(6, 0), CompassDirection.North));

            Assert.Equal("position", ex.ParamName);
        }

        [Fact]
        public void IsPositionFree_ReflectsDeployedRovers()
        {
            IPlateau plateau = new Plateau(new Position(5, 5));
            Assert.True(plateau.IsPositionFree(new Position(2, 2)));

            plateau.Deploy(new Position(2, 2), CompassDirection.North);

            Assert.False(plateau.IsPositionFree(new Position(2, 2)));
        }
    }
}
