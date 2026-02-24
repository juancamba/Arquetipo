using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using Arquetipo.Api.Configuration;
using Arquetipo.Infrastructure.Extensions;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace Arquetipo.Api;


public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RateLimitSettings>(
            configuration.GetSection("RateLimiting"));

        var settings = configuration
            .GetSection("RateLimiting")
            .Get<RateLimitSettings>()!;

        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("Fixed", opt =>
            {
                opt.PermitLimit = settings.PermitLimit;
                opt.Window = TimeSpan.FromSeconds(settings.WindowSeconds);
                opt.QueueLimit = settings.QueueLimit;
            });

            options.AddPolicy("PerUser", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    context.User.Identity?.Name ??
                    context.Connection.RemoteIpAddress?.ToString() ??
                    "anon",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = settings.PermitLimit,
                        Window = TimeSpan.FromSeconds(settings.WindowSeconds),
                        QueueLimit = settings.QueueLimit
                    }));
        });
        
        services.AddHealthChecks();
        services.AddInfrastructureHealthChecks();
        return services;
    }

    
    public static WebApplication UseApi(this WebApplication app)
    {
        app.UseRateLimiter();

        var settings = app.Services
            .GetRequiredService<IOptions<RateLimitSettings>>()
            .Value;

        app.MapControllers()
           .RequireRateLimiting(settings.Policy);


        
    
        app.MapHealthChecks("/health");   

        return app;
    }
}
