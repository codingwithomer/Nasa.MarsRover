using Nasa.MarsRoboticRover.Application;
using Nasa.MarsRoboticRover.Application.Commands;
using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Domain;
using System;
using Xunit;

namespace Nasa.MarsRoboticRover.Test.Application
{
    public class MissionExecutorTests
    {
        private readonly IMissionExecutor _missionExecutor = new MissionExecutor();
        private readonly IParser _parser = TestParsers.CreateDefault();

        private string Run(string input)
        {
            return _missionExecutor.Execute(_parser.Parse(input));
        }

        [Fact]
        public void Execute_DeployingBeforeDefiningThePlateau_Throws()
        {
            ICommand[] commands = { new DeployRoverCommand(new Position(1, 2), CompassDirection.North) };

            Assert.Throws<InvalidOperationException>(() => _missionExecutor.Execute(commands));
        }

        [Fact]
        public void Execute_MovingBeforeAnyRoverIsDeployed_Throws()
        {
            ICommand[] commands = { new DefinePlateauCommand(new Position(5, 5)), new MoveRoverCommand() };

            Assert.Throws<InvalidOperationException>(() => _missionExecutor.Execute(commands));
        }

        [Fact]
        public void Execute_WithoutCommands_ReportsNothing()
        {
            Assert.Equal(string.Empty, _missionExecutor.Execute(Array.Empty<ICommand>()));
        }

        [Fact]
        public void Execute_PlateauOnly_ReportsNothing()
        {
            Assert.Equal(string.Empty, Run("5 5"));
        }

        [Fact]
        public void Execute_ReportsTheFinalStateOfEachRoverInDeploymentOrder()
        {
            string report = Run("5 5\n1 2 N\nM\n3 3 E\nR");

            Assert.Equal(string.Join(Environment.NewLine, "1 3 N", "3 3 S", ""), report);
        }

        [Fact]
        public void Execute_CalledTwice_DoesNotLeakStateBetweenMissions()
        {
            ICommand[] mission =
            {
                new DefinePlateauCommand(new Position(5, 5)),
                new DeployRoverCommand(new Position(1, 1), CompassDirection.North)
            };

            Assert.Equal(_missionExecutor.Execute(mission), _missionExecutor.Execute(mission));
        }

        [Fact]
        public void Execute_RoverDeployedOutsideThePlateau_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Run("5 5\n6 1 N"));
        }

        [Fact]
        public void Execute_TwoRoversOnTheSameSquare_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => Run("5 5\n1 1 N\n1 1 E"));
        }

        [Fact]
        public void Execute_RoverLeavingASquare_LetsTheNextRoverUseIt()
        {
            string report = Run("5 5\n1 1 N\nM\n1 1 E");

            Assert.Equal(string.Join(Environment.NewLine, "1 2 N", "1 1 E", ""), report);
        }

        [Fact]
        public void Execute_WhenALaterRoverFails_NoPartialReportIsProduced()
        {
            Assert.Throws<InvalidOperationException>(() => Run("5 5\n1 1 N\nM\n1 1 S\nMM"));
        }
    }
}
