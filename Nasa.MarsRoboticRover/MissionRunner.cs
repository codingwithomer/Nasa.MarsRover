using Nasa.MarsRoboticRover.Application.Interfaces;

namespace Nasa.MarsRoboticRover
{
    public class MissionRunner
    {
        private readonly IMissionInputProvider _inputProvider;
        private readonly IParser _parser;
        private readonly ICommandCenter _commandCenter;

        public MissionRunner(IMissionInputProvider inputProvider, IParser parser, ICommandCenter commandCenter)
        {
            _inputProvider = inputProvider;
            _parser = parser;
            _commandCenter = commandCenter;
        }

        public string Run()
        {
            string input = _inputProvider.GetInput();
            string results = _commandCenter.ExecuteCommands(_parser.Parse(input));

            return MissionReportFormatter.Format(input, results);
        }
    }
}
