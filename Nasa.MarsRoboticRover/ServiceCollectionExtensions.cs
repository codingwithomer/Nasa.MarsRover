using Microsoft.Extensions.DependencyInjection;
using Nasa.MarsRoboticRover.BLL;
using Nasa.MarsRoboticRover.BLL.Interfaces;
using Nasa.MarsRoboticRover.BLL.Parsing;

namespace Nasa.MarsRoboticRover
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMarsRover(this IServiceCollection services)
        {
            // All services are stateless: mission state lives in a MissionContext created per run.
            services.AddSingleton<IMissionInputProvider, SampleMissionInputProvider>();
            services.AddSingleton<ILineParser, PlateauLineParser>();
            services.AddSingleton<ILineParser, RoverLineParser>();
            services.AddSingleton<ILineParser, InstructionLineParser>();
            services.AddSingleton<IParser, CommandParser>();
            services.AddSingleton<ICommandCenter, CommandCenter>();
            services.AddSingleton<MissionRunner>();

            return services;
        }
    }
}
