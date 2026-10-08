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

            using IServiceScope scope = serviceProvider.CreateScope();

            Console.WriteLine(scope.ServiceProvider.GetRequiredService<MissionRunner>().Run());

            if (!Console.IsInputRedirected)
            {
                Console.ReadKey();
            }
        }
    }
}
