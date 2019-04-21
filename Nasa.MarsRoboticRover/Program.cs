using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.DependencyResolvers;
using Nasa.MarsRoboticRover.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace Nasa.MarsRoboticRover
{
    public class Program
    {
        private static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().Build();

            IServiceCollection services = new ServiceCollection();

            Ioc.ConfigureServices(services, configuration);

            string commandInput = Ioc.GetService<ICommandCenter>().SetCommandInputs();

            List<ICommand> commands = Ioc.GetService<IParser>().Parse(commandInput);

            Ioc.GetService<ICommandCenter>().ExecuteCommands(commands);

            Console.WriteLine(Ioc.GetService<ICommandCenter>().GetReportOutput());

            Console.ReadKey();
        }
    }
}
