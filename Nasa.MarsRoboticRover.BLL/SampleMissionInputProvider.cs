using Nasa.MarsRoboticRover.BLL.Interfaces;
using System.Text;

namespace Nasa.MarsRoboticRover.BLL
{
    public class SampleMissionInputProvider : IMissionInputProvider
    {
        public string GetInput()
        {
            StringBuilder commandString = new StringBuilder();
            commandString.AppendLine("5 5");
            commandString.AppendLine("1 2 N");
            commandString.AppendLine("LMLMLMLMM");
            commandString.AppendLine("3 3 E");
            commandString.Append("MMRMMRMRRM");

            return commandString.ToString();
        }
    }
}
