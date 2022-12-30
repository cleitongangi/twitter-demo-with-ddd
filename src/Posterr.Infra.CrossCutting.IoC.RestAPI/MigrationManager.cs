using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Posterr.Infra.Data.Context;

namespace Posterr.Infra.CrossCutting.IoC.RestAPI
{
    public static class MigrationManager
    {
        public static void ApplyMigration(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<IPosterrDbContext>();
            appContext.Database.Migrate();
        }
    }
}
