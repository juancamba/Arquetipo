using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Arquetipo.Domain.Abstractions;
using NetArchTest.Rules;

namespace Arquetipo.Architecture.UnitTest.Infrastructure
{
    public class InfrastructureTests
    {
        private readonly Assembly infrastructureAssembly = typeof(Arquetipo.Infrastructure.ApplicationDbContext).Assembly;

        // =========================================
        // REPOSITORIES
        // =========================================

        [Fact]
        public void Repositories_Should_Implement_IRepository()
        {
            var result = Types
                .InAssembly(infrastructureAssembly)
                .That()
                .HaveNameEndingWith("Repository")
                .Should()
                .ImplementInterface(typeof(IRepository<,>))
                .GetResult();

            Assert.True(result.IsSuccessful, BuildMessage(result, nameof(Repositories_Should_Implement_IRepository)));
        }


        [Fact]
        public void Repositories_Should_Not_Be_Public()
        {
            var result = Types
                .InAssembly(infrastructureAssembly)
                .That()
                .HaveNameEndingWith("Repository")
                .Should()
                .NotBePublic()
                .GetResult();

            Assert.True(result.IsSuccessful, BuildMessage(result, nameof(Repositories_Should_Not_Be_Public)));
        }


    


        // =========================================
        // NO LÓGICA DE APLICACIÓN
        // =========================================

        [Fact]
        public void Infrastructure_Should_Not_Have_CommandHandlers()
        {
            var result = Types
                .InAssembly(infrastructureAssembly)
                .ShouldNot()
                .HaveNameEndingWith("Handler")
                .GetResult();

            Assert.True(result.IsSuccessful, BuildMessage(result, nameof(Infrastructure_Should_Not_Have_CommandHandlers)));
        }


        [Fact]
        public void Infrastructure_Should_Not_Have_Controllers()
        {
            var result = Types
                .InAssembly(infrastructureAssembly)
                .ShouldNot()
                .HaveNameEndingWith("Controller")
                .GetResult();

            Assert.True(result.IsSuccessful, BuildMessage(result, nameof(Infrastructure_Should_Not_Have_Controllers)));
        }


        // =========================================
        // DEPENDENCY RULES
        // =========================================

        [Fact]
        public void Infrastructure_Should_Not_Depend_On_Api()
        {
            var result = Types
                .InAssembly(infrastructureAssembly)
                .ShouldNot()
                .HaveDependencyOn("Api")
                .GetResult();

            Assert.True(result.IsSuccessful, BuildMessage(result, nameof(Infrastructure_Should_Not_Depend_On_Api)));
        }


        // =========================================
        // HELPERS
        // =========================================

        private static string BuildMessage(TestResult result, string testName)
        {
            if (result.IsSuccessful)
                return string.Empty;

            return $"{testName} fail:\n" +
                   string.Join("\n", result.FailingTypeNames);
        }
    }
}