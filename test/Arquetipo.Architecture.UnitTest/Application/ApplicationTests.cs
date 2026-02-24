
using System.Reflection;
using Arquetipo.Application.Shared;


using Mediator;
using NetArchTest.Rules;
using Xunit;

namespace Arquetipo.ArchitectureTests.Application;

public class ApplicationTests
{

    Assembly applicationAssembly = typeof(ApplicationMarker).Assembly;

    [Fact]
    public void CommandHandler_Should_NotBePublic()
    {


        var handlers1 = Types.InAssembly(applicationAssembly)
             .That()
             .AreClasses()
             .And()
             .ImplementInterface(typeof(ICommandHandler<>))
             .Should()
             .NotBePublic()
             .GetResult();

        var handlers2 = Types.InAssembly(applicationAssembly)
            .That()
            .AreClasses()
            .And()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .NotBePublic()
            .GetResult();

        // Asegurar que FailingTypes nunca sea null
        var failing1 = handlers1?.FailingTypes ?? Enumerable.Empty<Type>();
        var failing2 = handlers2?.FailingTypes ?? Enumerable.Empty<Type>();

        var failing = failing1.Concat(failing2).ToList();

        if (failing.Any())
        {
            var nombres = string.Join(", ", failing.Select(t => t.FullName));
            throw new Exception($"Existen CommandHandlers públicos: {nombres}");
        }


        Assert.True(true); // todo correcto
    }


    [Fact]
    public void QueryHandler_Should_NotBePublic()
    {
        var resultados = Types.InAssembly(applicationAssembly)
        .That()
        .ImplementInterface(typeof(IQueryHandler<,>))
        .Should()
        .NotBePublic()
        .GetResult();

        Assert.True(resultados.IsSuccessful, nameof(QueryHandler_Should_NotBePublic));
    }
    [Fact]
    public void Application_Should_Not_Have_Access_To_EntityFrameworkCore()
    {
        var result = Types.InAssembly(applicationAssembly)

          .ShouldNot().HaveDependencyOn("Microsoft.EntityFrameworkCore")
          .GetResult();
        Assert.True(result.IsSuccessful, nameof(Application_Should_Not_Have_Access_To_EntityFrameworkCore));
    }

}