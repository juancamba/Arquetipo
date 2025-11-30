

using Arquetipo.Application.Behaviors;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Arquetipo.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddMediator((MediatorOptions options) =>
        {

            options.PipelineBehaviors = [typeof(ValidationBehavior<,>)];
        });
        return services;
    }
}