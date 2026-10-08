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
        public void Execute_DeployingBeforeDefiningThePlateau_FailsWithTheMissionProblem()
        {
            ICommand[] commands = { new DeployRoverCommand(new Position(1, 2), CompassDirection.North) };

            Assert.Throws<InvalidMissionException>(() => _missionExecutor.Execute(commands));
        }

        [Fact]
        public void Execute_MovingBeforeAnyRoverIsDeployed_FailsWithTheMissionProblem()
        {
            ICommand[] commands = { new DefinePlateauCommand(new Position(5, 5)), new MoveRoverCommand() };

            Assert.Throws<InvalidMissionException>(() => _missionExecutor.Execute(commands));
        }

        [Fact]
        public void Execute_NullCommands_IsAProgrammingError()
        {
            Assert.Throws<ArgumentNullException>(() => _missionExecutor.Execute(null));
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
        public void Execute_RoverDeployedOutsideThePlateau_NamesTheRoverAndTheLine()
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => Run("5 5\n1 1 N\nM\n6 1 N\nM"));

            Assert.Equal("Rover 2 (line 4): (6, 1) is outside the plateau.", ex.Message);
        }

        [Fact]
        public void Execute_RoverMovingOffThePlateau_NamesTheRoverTheLineAndTheSquares()
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => Run("5 5\n1 1 N\nM\n2 4 N\nMM"));

            Assert.Equal("Rover 2 (line 5): (2, 6) is outside the plateau; it cannot move North from (2, 5).", ex.Message);
        }

        [Fact]
        public void Execute_TwoRoversOnTheSameSquare_NamesTheRoverAndTheLine()
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => Run("5 5\n1 1 N\nL\n1 1 E\nM"));

            Assert.Equal("Rover 2 (line 4): (1, 1) is already occupied by another rover.", ex.Message);
        }

        [Fact]
        public void Execute_RoverBlockedByAnotherRover_NamesTheRoverAndTheLine()
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => Run("5 5\n1 2 N\nL\n1 1 N\nM"));

            Assert.Equal("Rover 2 (line 5): (1, 2) is occupied by another rover; it cannot move North from (1, 1).", ex.Message);
        }

        [Fact]
        public void Execute_RoverNextToIntMaxValue_FailsCleanlyInsteadOfWrappingAround()
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => Run("2147483647 5\n2147483647 0 E\nM"));

            Assert.Equal("Rover 1 (line 3): The next square is outside the plateau; it cannot move East from (2147483647, 0).", ex.Message);
        }

        [Fact]
        public void Execute_RoverLeavingASquare_LetsTheNextRoverUseIt()
        {
            string report = Run("5 5\n1 1 N\nM\n1 1 E\nLR");

            Assert.Equal(string.Join(Environment.NewLine, "1 2 N", "1 1 E", ""), report);
        }

        [Fact]
        public void Execute_WhenALaterRoverFails_TheFinishedRoversAreStillReported()
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => Run("5 5\n1 1 N\nM\n3 3 E\nM\n5 5 N\nM"));

            Assert.Equal(string.Join(Environment.NewLine, "1 2 N", "4 3 E", ""), ex.PartialReport);
            Assert.StartsWith("Rover 3 (line 7)", ex.Message);
        }

        [Fact]
        public void Execute_WhenTheFirstRoverFails_ThePartialReportIsEmpty()
        {
            InvalidMissionException ex = Assert.Throws<InvalidMissionException>(() => Run("5 5\n9 9 N\nM"));

            Assert.Equal(string.Empty, ex.PartialReport);
        }
    }
}
