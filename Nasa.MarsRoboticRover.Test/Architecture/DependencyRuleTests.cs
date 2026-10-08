using Nasa.MarsRoboticRover.Application;
using Nasa.MarsRoboticRover.Domain;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Nasa.MarsRoboticRover.Test.Architecture
{
    /// <summary>Dependencies must point inwards: Console -> Application -> Domain.</summary>
    public class DependencyRuleTests
    {
        private static readonly Assembly DomainAssembly = typeof(Position).Assembly;
        private static readonly Assembly ApplicationAssembly = typeof(CommandCenter).Assembly;
        private static readonly Assembly ConsoleAssembly = typeof(MissionRunner).Assembly;

        private static string[] ReferencedProjects(Assembly assembly)
        {
            return assembly.GetReferencedAssemblies()
                           .Select(reference => reference.Name)
                           .Where(name => name.StartsWith("Nasa.MarsRoboticRover"))
                           .ToArray();
        }

        [Fact]
        public void Domain_DependsOnNoOtherProject()
        {
            Assert.Empty(ReferencedProjects(DomainAssembly));
        }

        [Fact]
        public void Application_DependsOnDomainOnly()
        {
            Assert.Equal(new[] { DomainAssembly.GetName().Name }, ReferencedProjects(ApplicationAssembly));
        }

        [Fact]
        public void Application_DoesNotReferenceTheDependencyInjectionContainer()
        {
            Assert.DoesNotContain(ApplicationAssembly.GetReferencedAssemblies(),
                reference => reference.Name.StartsWith("Microsoft.Extensions.DependencyInjection"));
        }

        [Fact]
        public void Console_IsNotReferencedByTheInnerLayers()
        {
            string consoleName = ConsoleAssembly.GetName().Name;

            Assert.DoesNotContain(consoleName, ReferencedProjects(ApplicationAssembly));
            Assert.DoesNotContain(consoleName, ReferencedProjects(DomainAssembly));
        }
    }
}
