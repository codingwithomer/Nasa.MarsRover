using Nasa.MarsRoboticRover.Application;
using Nasa.MarsRoboticRover.Application.Parsing;

namespace Nasa.MarsRoboticRover.Test.Application
{
    internal static class TestParsers
    {
        public static CommandParser CreateDefault()
        {
            return new CommandParser(new ILineParser[]
            {
                new PlateauLineParser(),
                new RoverLineParser(),
                new InstructionLineParser()
            });
        }
    }
}
