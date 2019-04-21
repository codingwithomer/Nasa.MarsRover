using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nasa.MarsRoboticRover.BLL;
using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities;
using Nasa.MarsRoboticRover.Entities.Interfaces;

namespace Nasa.MarsRoboticRover.DependencyResolvers
{
    public static class Ioc
    {
        private static ServiceProvider _serviceProvider;

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IParser, CommandParser>();
            services.AddScoped<ICommandCenter, CommandCenter>();
            services.AddScoped<ILocation, Plateau>();

            _serviceProvider = services.BuildServiceProvider();
        }

        public static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
