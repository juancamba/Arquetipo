
using System.Reflection;
using Arquetipo.Domain.Abstractions;
using NetArchTest.Rules;


namespace Arquetipo.Architecture.UnitTest.Domain
{
    public class DomainArchitectureTests
    {
        Assembly domainAssembly = typeof(Entity<>).Assembly;

        [Fact]
        public void Domain_Entities_Should_Be_Sealed()
        {


            var result = Types
                .InAssembly(domainAssembly)
                .That()
                .Inherit(typeof(Entity<>))
                .Should()
                .BeSealed()
                .GetResult();

            Assert.True(result.IsSuccessful, BuildMessage(result, "Domain_Entities_Should_Be_Sealed"));
        }

        [Fact]
        public void Domain_Entities_Should_Not_Be_Public_Setters()
        {
            var failing = domainAssembly
            .GetTypes()
            .Where(t => typeof(IEntity).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetProperties())
            .Where(p =>
            {
                var setMethod = p.SetMethod;

                if (setMethod == null)
                    return false;

                // permitir init
                var isInitOnly = setMethod.ReturnParameter
                    .GetRequiredCustomModifiers()
                    .Any(m => m.Name == "IsExternalInit");

                return setMethod.IsPublic && !isInitOnly;
            })
            .Select(p => $"{p.DeclaringType!.FullName}.{p.Name}")
            .ToList();

            Assert.True(!failing.Any(), nameof(Domain_Entities_Should_Not_Be_Public_Setters) + string.Join("\n", failing));
        }
        [Fact]
        public void Domain_Events_Should_Follow_Convention()
        {
            var result = Types
                .InAssembly(domainAssembly)
                .That()
                .AreClasses()
                .And()
                .ImplementInterface(typeof(IDomainEvent))
                .Should()
                .BeSealed()
                .And()
                .HaveNameEndingWith("DomainEvent")
                .GetResult();

            Assert.True(result.IsSuccessful, BuildMessage(result, nameof(Domain_Events_Should_Follow_Convention)));
        }
        // ================================
        // DEPENDENCY RULES (CRÍTICAS)
        // ================================

        [Fact]
        public void Domain_Should_Not_Depend_On_Application()
        {
            var result = Types
                .InAssembly(domainAssembly)
                .ShouldNot()
                .HaveDependencyOn("Application")
                .GetResult();

            Assert.True(result.IsSuccessful, BuildMessage(result, nameof(Domain_Should_Not_Depend_On_Application)));
        }

        [Fact]
        public void Domain_Should_Not_Depend_On_Infrastructure()
        {
            var result = Types
                .InAssembly(domainAssembly)
                .ShouldNot()
                .HaveDependencyOn("Infrastructure")
                .GetResult();

            Assert.True(result.IsSuccessful, BuildMessage(result, nameof(Domain_Should_Not_Depend_On_Infrastructure)));
        }

        [Fact]
        public void Domain_Should_Not_Depend_On_Api()
        {
            var result = Types
                .InAssembly(domainAssembly)
                .ShouldNot()
                .HaveDependencyOn("Api")
                .GetResult();

            Assert.True(result.IsSuccessful, BuildMessage(result, nameof(Domain_Should_Not_Depend_On_Api)));
        }


        // ================================
        // PUREZA DEL DOMINIO
        // ================================

        [Fact]
        public void Domain_Should_Only_Depend_On_System_Domain_And_AllowedPackages()
        {
            // Assembly del Domain

            var domainName = domainAssembly.GetName().Name;

            // Assemblies prohibidos
            var namespacePrefix = typeof(Entity<>).Namespace?.Split('.')[0]; // ejemplo: "Arquetipo"

            var forbiddenAssemblies = new string[]
            {
                $"{namespacePrefix}.Application",
                $"{namespacePrefix}.Infrastructure",
                $"{namespacePrefix}.Api"
            };

            // Assemblies permitidos
            var allowedAssemblies = new string[]
            {
                "System",
                domainName!,
                "Mediator" // Librería Mediator, si está permitida
            };

            // Verificar que no dependa de proyectos prohibidos
            var forbiddenResult = Types
                .InAssembly(domainAssembly)
                .ShouldNot()
                .HaveDependencyOnAny(forbiddenAssemblies)
                .GetResult();

            Assert.True(forbiddenResult.IsSuccessful, BuildMessage(forbiddenResult, "forbidden dependencies detected!"));

            // Verificar que solo dependa de los permitidos
            var whitelistResult = Types
                .InAssembly(domainAssembly)
                .Should()
                .OnlyHaveDependenciesOn(allowedAssemblies)
                .GetResult();

            Assert.True(whitelistResult.IsSuccessful, BuildMessage(whitelistResult, "dependencies not allowed detected"));
        }
        private static string BuildMessage(TestResult result, string prefix)
        {
            if (result.IsSuccessful) return string.Empty;

            return prefix + ":\n" +
                   string.Join("\n", result.FailingTypeNames);
        }




    }
}