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

            var commandCenter = Ioc.GetService<ICommandCenter>();

            var commandInput = commandCenter.SetCommandInputs();

            var commandParser = Ioc.GetService<IParser>();

            var commands = commandParser.Parse(commandInput);

            commandCenter.ExecuteCommands(commands);

            var reportOutput = commandCenter.GetReportOutput();

            Console.WriteLine(reportOutput);

            Console.ReadKey();
        }
    }
}
