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

        public string Run()
        {
            string input = _inputProvider.GetInput();
            string results = _missionExecutor.Execute(_parser.Parse(input));

            return MissionReportFormatter.Format(input, results);
        }
    }
}
