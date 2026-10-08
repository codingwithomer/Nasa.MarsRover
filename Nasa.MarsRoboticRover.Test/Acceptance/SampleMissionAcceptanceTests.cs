using Nasa.MarsRoboticRover.Application;
using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Domain;
using Nasa.MarsRoboticRover.Test.Application;
using System;
using System.Collections.Generic;
using Xunit;

namespace Nasa.MarsRoboticRover.Test.Acceptance
{
    public class SampleMissionAcceptanceTests
    {
        [Fact]
        public void CommandParser_Should_GenerateCommandsAndOutput()
        {
            IParser commandParser = TestParsers.CreateDefault();
            ICommandCenter commandCenter = new CommandCenter();

            List<ICommand> commands = commandParser.Parse(new SampleMissionInputProvider().GetInput());
            string output = commandCenter.ExecuteCommands(commands);

            string expectedString = string.Join(Environment.NewLine, "1 3 N", "5 1 E", "");
            Assert.Equal(expectedString, output);
        }

        [Fact]
        public void Rovers_ShouldEndUpWhereTheMissionBriefSays()
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
