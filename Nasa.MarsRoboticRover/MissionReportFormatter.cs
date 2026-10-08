using System;

namespace Nasa.MarsRoboticRover
{
    public static class MissionReportFormatter
    {
        public static string Format(string input, string results)
        {
            string newLine = Environment.NewLine;

            return $"Test Input:{newLine}{input}{newLine}{newLine}{newLine}Expected Output:{newLine}{results}";
        }
    }
}
