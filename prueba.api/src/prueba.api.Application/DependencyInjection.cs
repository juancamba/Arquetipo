using FluentValidation;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using prueba.api.Application.Alquiler.CrearAlquiler;
using prueba.api.Application.Behaviors;

namespace prueba.api.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services, IConfiguration configuration)
    {

        // Detectar namespace raíz automáticamente

        //services.AddMediator();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddMediator((MediatorOptions options) =>
        {
            //options.Assemblies = types;
            //options.PipelineBehaviors = [typeof(CrearAlquilerValidator)];
            options.PipelineBehaviors = [typeof(ValidationBehavior<,>)];
        });
        
        return services;
    }
}