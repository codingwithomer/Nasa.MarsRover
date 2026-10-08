using Nasa.MarsRoboticRover.Application;
using Nasa.MarsRoboticRover.Domain;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Xunit;

namespace Nasa.MarsRoboticRover.Test.Architecture
{
    /// <summary>
    /// Dependencies must point inwards: Console -> Application -> Domain.
    /// The rule is checked on the project files (what is declared) and on the compiled assemblies (what is used).
    /// </summary>
    public class DependencyRuleTests
    {
        private const string Domain = "Nasa.MarsRoboticRover.Domain";
        private const string ApplicationLayer = "Nasa.MarsRoboticRover.Application";
        private const string ConsoleLayer = "Nasa.MarsRoboticRover";

        private static readonly Assembly DomainAssembly = typeof(Position).Assembly;
        private static readonly Assembly ApplicationAssembly = typeof(MissionExecutor).Assembly;
        private static readonly Assembly ConsoleAssembly = typeof(ConsoleApplication).Assembly;

        private static XElement[] References(string project, string kind)
        {
            string directory = AppContext.BaseDirectory;
            while (!File.Exists(Path.Combine(directory, "Nasa.sln")))
            {
                directory = Directory.GetParent(directory)?.FullName
                    ?? throw new InvalidOperationException("Nasa.sln was not found above the test output folder.");
            }

            return XDocument.Load(Path.Combine(directory, project, project + ".csproj"))
                            .Descendants(kind)
                            .ToArray();
        }

        private static string[] ReferencedProjects(string project)
        {
            return References(project, "ProjectReference")
                .Select(reference => Path.GetFileNameWithoutExtension(reference.Attribute("Include").Value.Replace('\\', '/')))
                .OrderBy(name => name)
                .ToArray();
        }

        private static string[] ReferencedPackages(string project)
        {
            return References(project, "PackageReference").Select(reference => reference.Attribute("Include").Value).ToArray();
        }

        [Fact]
        public void Domain_DeclaresNoProjectOrPackageReferences()
        {
            Assert.Empty(ReferencedProjects(Domain));
            Assert.Empty(ReferencedPackages(Domain));
        }

        [Fact]
        public void Application_DeclaresOnlyTheDomainProject_AndNoPackages()
        {
            Assert.Equal(new[] { Domain }, ReferencedProjects(ApplicationLayer));
            Assert.Empty(ReferencedPackages(ApplicationLayer));
        }

        [Fact]
        public void Console_DeclaresOnlyTheApplicationProject()
        {
            Assert.Equal(new[] { ApplicationLayer }, ReferencedProjects(ConsoleLayer));
        }

        [Fact]
        public void Console_DeclaresOnlyTheDependencyInjectionPackage()
        {
            Assert.Equal(new[] { "Microsoft.Extensions.DependencyInjection" }, ReferencedPackages(ConsoleLayer));
        }

        [Fact]
        public void Console_UsesTheApplicationButNeverTheDomainAssembly()
        {
            string[] others = ConsoleAssembly.GetReferencedAssemblies()
                .Select(reference => reference.Name)
                .Where(name => !name.StartsWith("System") && name != "netstandard" && name != "mscorlib" &&
                               !name.StartsWith("Microsoft.Extensions") && name != ApplicationLayer)
                .ToArray();

            Assert.Empty(others);
        }

        [Fact]
        public void Domain_UsesOnlyFrameworkAssemblies()
        {
            string[] others = DomainAssembly.GetReferencedAssemblies()
                .Select(reference => reference.Name)
                .Where(name => !name.StartsWith("System") && name != "netstandard" && name != "mscorlib")
                .ToArray();

            Assert.Empty(others);
        }

        [Fact]
        public void Application_UsesNoThirdPartyOrExtensionsAssemblies()
        {
            string[] others = ApplicationAssembly.GetReferencedAssemblies()
                .Select(reference => reference.Name)
                .Where(name => !name.StartsWith("System") && name != "netstandard" && name != "mscorlib" && name != Domain)
                .ToArray();

            Assert.Empty(others);
        }

        [Theory]
        [InlineData(Domain)]
        [InlineData(ApplicationLayer)]
        public void EveryTypeLivesInItsLayersNamespace(string layer)
        {
            Assembly assembly = layer == Domain ? DomainAssembly : ApplicationAssembly;

            string[] misplaced = assembly.GetTypes()
                .Where(type => !type.Name.StartsWith("<") && !type.Namespace.StartsWith(layer))
                .Select(type => type.FullName)
                .ToArray();

            Assert.Empty(misplaced);
        }
    }
}
