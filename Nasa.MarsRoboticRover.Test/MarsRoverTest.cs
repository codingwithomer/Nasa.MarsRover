using Nasa.MarsRoboticRover.BLL;
using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities;
using Nasa.MarsRoboticRover.Entities.Interfaces;
using System;
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
            string expectedString = string.Join(Environment.NewLine, "Test Input:", "5 5", "1 2 N", "LMLMLMLMM", "3 3 E", "MMRMMRMRRM", "", "", "Expected Output:", "1 3 N", "5 1 E", "");
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
            string expectedString = string.Join(Environment.NewLine, "1 3 N", "5 1 E", "");
            Assert.Equal(expectedString, output);
        }
    }
}
