using Microsoft.Extensions.DependencyInjection;
using System;

namespace Nasa.MarsRoboticRover
{
    public class Program
    {
        private static void Main(string[] args)
        {
            using ServiceProvider serviceProvider = new ServiceCollection()
                .AddMarsRover()
                .BuildServiceProvider(validateScopes: true);

            Console.WriteLine(serviceProvider.GetRequiredService<MissionRunner>().Run());

            if (!Console.IsInputRedirected)
            {
                Console.ReadKey();
            }
        }
    }
}
