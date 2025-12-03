
using Arquetipo.ArchitectureTests.Infrastructure;
using FluentAssertions;
using Mediator;
using NetArchTest.Rules;
using Xunit;

namespace Arquetipo.ArchitectureTests.Application;

public class ApplicationTests : BaseTest
{

    [Fact]
    public void CommandHandler_Should_NotBePublic()
    {
       var handlers1 = Types.InAssembly(ApplicationAssembly)
            .That()
            .AreClasses()
            .And()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Should()
            .NotBePublic()
            .GetResult();

        var handlers2 = Types.InAssembly(ApplicationAssembly)
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
        var resultados = Types.InAssembly(ApplicationAssembly)
        .That()
        .ImplementInterface(typeof(IQueryHandler<,>))
        .Should()
        .NotBePublic()
        .GetResult();

        Assert.True(resultados.IsSuccessful, "Existen QueryHandlers públicos y no deberían serlo.");
    }


}