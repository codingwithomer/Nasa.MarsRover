using System;

namespace Nasa.MarsRoboticRover
{
    public class Program
    {
        private static int Main(string[] args)
        {
            int exitCode = ConsoleApplication.Run(args, Console.In, Console.IsInputRedirected, Console.Out, Console.Error);

            if (ConsoleApplication.ShouldWaitForKey(args, Console.IsInputRedirected, exitCode))
            {
                Console.ReadKey();
            }

            return exitCode;
        }
    }
}
