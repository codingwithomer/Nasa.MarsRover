using Nasa.MarsRoboticRover.BLL;
using Nasa.MarsRoboticRover.BLL.Parsing;

namespace Nasa.MarsRoboticRover.Test
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
