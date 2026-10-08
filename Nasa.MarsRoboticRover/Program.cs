using System;

namespace Nasa.MarsRoboticRover
{
    public class Program
    {
        private static int Main(string[] args)
        {
            int exitCode = ConsoleApplication.Run(args, Console.In, Console.IsInputRedirected, Console.Out, Console.Error);

            if (!Console.IsInputRedirected)
            {
                Console.ReadKey();
            }

            return exitCode;
        }
    }
}
