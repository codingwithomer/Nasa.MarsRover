using Nasa.MarsRoboticRover.Domain;
using Xunit;

namespace Nasa.MarsRoboticRover.Test.Domain
{
    public class MissionBriefRoverTests
    {
        [Fact]
        public void Rovers_DrivenDirectly_EndUpWhereTheMissionBriefSays()
        {
            Plateau plateau = new Plateau(new Position(5, 5));

            MarsRover rover1 = plateau.Deploy(new Position(1, 2), CompassDirection.North);
            rover1.Rotate(Rotation.Left);
            rover1.Move();
            rover1.Rotate(Rotation.Left);
            rover1.Move();
            rover1.Rotate(Rotation.Left);
            rover1.Move();
            rover1.Rotate(Rotation.Left);
            rover1.Move();
            rover1.Move();

            MarsRover rover2 = plateau.Deploy(new Position(3, 3), CompassDirection.East);
            rover2.Move();
            rover2.Move();
            rover2.Rotate(Rotation.Right);
            rover2.Move();
            rover2.Move();
            rover2.Rotate(Rotation.Right);
            rover2.Move();
            rover2.Rotate(Rotation.Right);
            rover2.Rotate(Rotation.Right);
            rover2.Move();

            Assert.Equal((new Position(1, 3), CompassDirection.North), (rover1.Position, rover1.CompassDirection));
            Assert.Equal((new Position(5, 1), CompassDirection.East), (rover2.Position, rover2.CompassDirection));
        }
    }
}
