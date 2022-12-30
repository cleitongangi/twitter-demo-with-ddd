using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Posterr.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posterr.RestAPI.IntegrationTests
{
    internal class PosterrWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<PosterrDbContext>));
                services.AddDbContext<PosterrDbContext>(options =>
                        options
                        .UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=Posterr;Trusted_Connection=True;MultipleActiveResultSets=true")
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    );
            });

            return base.CreateHost(builder);
        }
    }
}
