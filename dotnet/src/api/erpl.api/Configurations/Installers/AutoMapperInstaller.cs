using AutoMapper;
using erpl.api.Configurations.AutoMappingProfiles.Locations;
using erpl.api.Configurations.AutoMappingProfiles.Suspects;
using erpl.common.infrastructure.Types;
using Microsoft.Extensions.DependencyInjection;

namespace erpl.api.Configurations.Installers;

internal static class AutoMapperInstaller
{
    public static IServiceCollection AddAutoMapperInstaller(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            // Suspect
            cfg.AddProfile<SuspectToSuspectUiAutoMapperProfile>();
            // Location
            cfg.AddProfile<LocationToLocationUiAutoMapperProfile>();
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddSingleton<IAutoMapper, AutoMapperAdapter>();

        return services;
    }
}//Class : AutoMapperInstaller