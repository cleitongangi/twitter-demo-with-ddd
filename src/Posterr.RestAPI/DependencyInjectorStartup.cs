using Posterr.Domain.Interfaces.GlobalSettings;
using Posterr.RestAPI.GlobalSettings;

namespace Posterr.RestAPI
{
    public static class DependencyInjectorStartup
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfigSettings, ConfigSettings>();

            Posterr.Infra.CrossCutting.IoC.RestAPI.IoCWrapper.RegisterIoC(services, configuration);
        }
    }
}
