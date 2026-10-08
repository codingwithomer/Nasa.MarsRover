using Microsoft.Extensions.DependencyInjection;
using Nasa.MarsRoboticRover.Application;
using Nasa.MarsRoboticRover.Application.Interfaces;
using Nasa.MarsRoboticRover.Application.Parsing;

namespace Nasa.MarsRoboticRover
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>Registers parsing and execution. The caller must also register one <see cref="IMissionInputProvider"/>.</summary>
        public static IServiceCollection AddMarsRover(this IServiceCollection services)
        {
            // All services are stateless: mission state lives in a MissionContext created per run.
            // The mission input is not registered here: the host registers exactly one IMissionInputProvider.
            services.AddSingleton<IInstructionSet>(_ => InstructionSet.CreateDefault());
            services.AddSingleton<ILineParser, PlateauLineParser>();
            services.AddSingleton<ILineParser, RoverLineParser>();
            services.AddSingleton<ILineParser, InstructionLineParser>();
            services.AddSingleton<IParser, CommandParser>();
            services.AddSingleton<IMissionExecutor, MissionExecutor>();
            services.AddSingleton<MissionRunner>();

            return services;
        }
    }
}
