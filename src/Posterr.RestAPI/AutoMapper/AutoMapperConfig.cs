using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Posterr.RestAPI.AutoMapper.Profiles;
using System;

namespace Posterr.RestAPI.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PosterrMappingProfile());                
            })
            .AssertConfigurationIsValid();
        }
    }
}
