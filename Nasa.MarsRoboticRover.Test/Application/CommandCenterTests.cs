using Nasa.MarsRoboticRover.Application;
using Nasa.MarsRoboticRover.Application.Commands;
using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Domain;
using System;
using Xunit;

namespace Nasa.MarsRoboticRover.Test.Application
{
    public class CommandCenterTests
    {
        private readonly ICommandCenter _commandCenter = new CommandCenter();

        [Fact]
        public void ExecuteCommands_DeployingBeforeDefiningThePlateau_Throws()
        {
            ICommand[] commands = { new DeployRoverCommand(new Position(1, 2), CompassDirection.North) };

            Assert.Throws<InvalidOperationException>(() => _commandCenter.ExecuteCommands(commands));
        }

        [Fact]
        public void ExecuteCommands_MovingBeforeAnyRoverIsDeployed_Throws()
        {
            ICommand[] commands = { new DefinePlateauCommand(new Position(5, 5)), new MoveRoverCommand() };

            Assert.Throws<InvalidOperationException>(() => _commandCenter.ExecuteCommands(commands));
        }

        [Fact]
        public void ExecuteCommands_ReportsOneLinePerPrintCommand()
        {
            ICommand[] commands =
            {
                new DefinePlateauCommand(new Position(5, 5)),
                new DeployRoverCommand(new Position(1, 2), CompassDirection.North),
                new MoveRoverCommand(),
                new PrintPositionAndCompassDirectionCommand(),
                new RoverRotatorCommand(Rotation.Right),
                new PrintPositionAndCompassDirectionCommand()
            };

            string report = _commandCenter.ExecuteCommands(commands);

            Assert.Equal(string.Join(Environment.NewLine, "1 3 N", "1 3 E", ""), report);
        }

        [Fact]
        public void ExecuteCommands_CalledTwice_DoesNotLeakStateBetweenMissions()
        {
            ICommand[] mission =
            {
                new DefinePlateauCommand(new Position(5, 5)),
                new DeployRoverCommand(new Position(1, 1), CompassDirection.North),
                new PrintPositionAndCompassDirectionCommand()
            };

            Assert.Equal(_commandCenter.ExecuteCommands(mission), _commandCenter.ExecuteCommands(mission));
        }
    }
}
