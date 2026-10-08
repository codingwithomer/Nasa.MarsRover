using Microsoft.Extensions.DependencyInjection;
using Nasa.MarsRoboticRover.BLL;
using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.Entities;
using Nasa.MarsRoboticRover.Entities.Interfaces;

namespace Nasa.MarsRoboticRover
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMarsRover(this IServiceCollection services)
        {
            // Plateau holds the state of one mission, so everything that touches it is scoped:
            // one scope == one mission run.
            services.AddScoped<IMissionInputProvider, SampleMissionInputProvider>();
            services.AddScoped<IParser, CommandParser>();
            services.AddScoped<ICommandCenter, CommandCenter>();
            services.AddScoped<ILocation, Plateau>();
            services.AddScoped<MissionRunner>();

            return services;
        }
    }
}
