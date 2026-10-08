using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.DependencyResolvers;
using System;

namespace Nasa.MarsRoboticRover
{
    public class Program
    {
        private static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().Build();

            IServiceCollection services = new ServiceCollection();

            Ioc.ConfigureServices(services, configuration);

            var inputProvider = Ioc.GetService<IMissionInputProvider>();
            var commandParser = Ioc.GetService<IParser>();
            var commandCenter = Ioc.GetService<ICommandCenter>();

            var commandInput = inputProvider.GetInput();
            var commands = commandParser.Parse(commandInput);
            var results = commandCenter.ExecuteCommands(commands);

            Console.WriteLine(MissionReportFormatter.Format(commandInput, results));

            Console.ReadKey();
        }
    }
}
