using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquetipo.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Arquetipo.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task ApplyMigration(this IApplicationBuilder app)
    {
        // Obtener el entorno
        var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

        // Solo ejecutar en Development

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var service = scope.ServiceProvider;
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();
            try
            {
                var context = service.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
                logger.LogInformation("Migraciones aplicadas correctamente en {Environment}", env.EnvironmentName);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "Error en migracion");
            }
        }
    }
}
