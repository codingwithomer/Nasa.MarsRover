using Nasa.MarsRoboticRover.Application.Interfaces;

namespace Nasa.MarsRoboticRover
{
    public class MissionRunner
    {
        private readonly IMissionInputProvider _inputProvider;
        private readonly IParser _parser;
        private readonly IMissionExecutor _missionExecutor;

        public MissionRunner(IMissionInputProvider inputProvider, IParser parser, IMissionExecutor missionExecutor)
        {
            _inputProvider = inputProvider;
            _parser = parser;
            _missionExecutor = missionExecutor;
        }

        /// <summary>Reads, parses and executes the mission and returns the report: one line per rover.</summary>
        public string Run()
        {
            return _missionExecutor.Execute(_parser.Parse(_inputProvider.GetInput()));
        }
    }
}
