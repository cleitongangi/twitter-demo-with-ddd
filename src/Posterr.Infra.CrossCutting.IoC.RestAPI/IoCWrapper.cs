using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posterr.Infra.CrossCutting.IoC.RestAPI
{
    public static class IoCWrapper
    {
        public static void RegisterIoC(IServiceCollection services, IConfiguration configuration)
        {
            var posterrDbConnectionString = configuration.GetConnectionString("PosterrDb");
            Data.RegisterIoC.Register(services, posterrDbConnectionString);

            Domain.RegisterIoC.Register(services);
        }
    }
}
