


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Arquetipo.Domain.Abstractions;
using Arquetipo.Domain.Users;
using Arquetipo.Infrastructure.Repositories;
using Arquetipo.Application.Abstractions.Email;
using Arquetipo.Infrastructure.Outbox;
using Quartz;

namespace Arquetipo.Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        
         services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));
        services.AddQuartz();
        services.AddQuartzHostedService(options =>          
            options.WaitForJobsToComplete = true
        );
        services.ConfigureOptions<ProcessOutboxMessageSetup>();
        
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddTransient<IEmailService, EmailService>();

        return services;
    }

}