using Microsoft.Extensions.DependencyInjection;
using Posterr.Domain.Interfaces.Validations;
using Posterr.Domain.Validations;

namespace Posterr.Domain
{
    public static class RegisterIoC
    {
        public static void Register(IServiceCollection services)
        {
            #region Register Validations            
            services.AddScoped<INewPostValidation, NewPostValidation>();
            services.AddScoped<IFollowUserValidation, FollowUserValidation>();
            services.AddScoped<IUnfollowUserValidation, UnfollowUserValidation>();
            services.AddScoped<IRepostValidation, RepostValidation>();
            #endregion Register Validations

            #region Register all services
            var servicesAssembly = typeof(Interfaces.Services.IPostService).Assembly;
            var serviceRegistrations =
                from type in servicesAssembly.GetExportedTypes()
                where type.Namespace == "Posterr.Domain.Services"
                where type.GetInterfaces().Any()
                select new { Interface = type.GetInterfaces().Single(), Implementation = type };

            foreach (var reg in serviceRegistrations)
                services.AddScoped(reg.Interface, reg.Implementation);
            #endregion Register all services
        }
    }
}
