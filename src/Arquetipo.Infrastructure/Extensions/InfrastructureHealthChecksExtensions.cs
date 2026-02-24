
using Microsoft.Extensions.DependencyInjection;

namespace Arquetipo.Infrastructure.Extensions;
public static class InfrastructureHealthChecksExtensions
{
    public static IServiceCollection AddInfrastructureHealthChecks(
        this IServiceCollection services)
    {
        services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

        return services;
    }
}
