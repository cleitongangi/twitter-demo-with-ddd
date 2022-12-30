using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Posterr.Domain.Interfaces.Data;
using Posterr.Infra.Data.Context;
using Posterr.Infra.Data.Repositories.PosterrDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posterr.Infra.Data
{
    public static class RegisterIoC
    {
        public static readonly LoggerFactory MyLoggerFactory = new(new[] { new DebugLoggerProvider() });

        public static void Register(IServiceCollection services, string posterrDbConnectionString)
        {
            #region Register DBContext            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPosterrDbContext, PosterrDbContext>();

            services.AddDbContext<PosterrDbContext>(options =>
            {
                options
                    .UseSqlServer(posterrDbConnectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .UseLoggerFactory(MyLoggerFactory);
            });
            #endregion Register DBContext


            #region Register all repositories
            var repositoriesAssembly = typeof(UserRepository).Assembly;
            var repositoriesRegistrations =
                from type in repositoriesAssembly.GetExportedTypes()
                where type.Namespace == "Posterr.Infra.Data.Repositories.PosterrDb"
                where type.GetInterfaces().Any()
                select new { Interface = type.GetInterfaces().FirstOrDefault(), Implementation = type };

            foreach (var reg in repositoriesRegistrations)
                services.AddScoped(reg.Interface, reg.Implementation);
            #endregion Register all repositories
        }
    }
}
