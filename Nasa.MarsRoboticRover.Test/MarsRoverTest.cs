using Nasa.MarsRoboticRover.BLL;
using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities;
using Nasa.MarsRoboticRover.Entities.Interfaces;
using System.Collections.Generic;
using Xunit;

namespace Nasa.MarsRoboticRover.Test
{

    public class MarsRoverTest
    {
        private readonly string _testInput;
        private readonly ICommandCenter _commandCenter;
        private readonly ILocation _location;

        public MarsRoverTest()
        {
            _location = new Plateau();
            _commandCenter = new CommandCenter(_location);
            _testInput = _commandCenter.SetCommandInputs();
        }

        [Fact]
        public void CommandParser_Should_GenerateCommandsAndOutput()
        {
            IParser commandParser = new CommandParser();
            List<ICommand> commands = commandParser.Parse(_testInput);
            _commandCenter.ExecuteCommands(commands);
            string output = _commandCenter.GetReportOutput();
            string expectedString = "Test Input:\r\n5 5\r\n1 2 N\r\nLMLMLMLMM\r\n3 3 E\r\nMMRMMRMRRM\r\n\r\n\r\nExpected Output:\r\n1 3 N\r\n5 1 E\r\n";
            Assert.Equal(expectedString, output.ToString());
        }

        [Fact]
        public void Rover_Should_GenerateOutput()
        {
            _location.Initialize(new Position(5, 5));
            IRover rover1 = new MarsRover(new Position(1, 2), CompassDirection.North, _location);
            rover1.Rotate(Rotation.Left);
            rover1.Move();
            rover1.Rotate(Rotation.Left);
            rover1.Move();
            rover1.Rotate(Rotation.Left);
            rover1.Move();
            rover1.Rotate(Rotation.Left);
            rover1.Move();
            rover1.Move();

            IRover rover2 = new MarsRover(new Position(3, 3), CompassDirection.East, _location);
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

            string output = rover1.PrintPositionAndCompassDirection() + rover2.PrintPositionAndCompassDirection();
            string expectedString = "1 3 N\r\n5 1 E\r\n";
            Assert.Equal(expectedString, output);
        }
    }
}
