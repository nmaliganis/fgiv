using AutoMapper;
using erpl.api.Configurations.AutoMappingProfiles.Wiretaps;
using erpl.common.infrastructure.Types;
using Microsoft.Extensions.DependencyInjection;

namespace erpl.api.Configurations.Installers;

internal static class AutoMapperInstaller
{
    public static IServiceCollection AddAutoMapperInstaller(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            // Wiretap
            cfg.AddProfile<WiretapToWiretapUiAutoMapperProfile>();
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddSingleton<IAutoMapper, AutoMapperAdapter>();

        return services;
    }
}//Class : AutoMapperInstaller