using System.Configuration;
using System.Reflection;

using Arquetipo.Domain.Abstractions;
using Arquetipo.Infrastructure;
using Mediator;

namespace Arquetipo.ArchitectureTests.Infrastructure;

public class BaseTest
{
    //quien representa Application ? IBaseCommand

    protected static readonly Assembly ApplicationAssembly = typeof(IBaseCommand).Assembly;
    protected static readonly Assembly DomainAssembly = typeof(IEntity).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;

}