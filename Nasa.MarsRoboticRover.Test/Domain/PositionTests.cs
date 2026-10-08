using Nasa.MarsRoboticRover.Domain;
using Xunit;

namespace Nasa.MarsRoboticRover.Test.Domain
{
    public class PositionTests
    {
        [Fact]
        public void Positions_WithTheSameCoordinates_AreEqual()
        {
            Assert.Equal(new Position(2, 3), new Position(2, 3));
            Assert.True(new Position(2, 3) == new Position(2, 3));
            Assert.NotEqual(new Position(2, 3), new Position(3, 2));
        }

        [Fact]
        public void ToString_IsReadableInErrorMessages()
        {
            Assert.Equal("(2, 3)", new Position(2, 3).ToString());
        }

        [Theory]
        [InlineData(0, 0, true)]
        [InlineData(5, 5, true)]
        [InlineData(6, 0, false)]
        [InlineData(0, -1, false)]
        public void IsWithin_IsInclusiveOnBothCorners(int x, int y, bool expected)
        {
            Assert.Equal(expected, new Position(x, y).IsWithin(Position.Origin, new Position(5, 5)));
        }
    }
}
