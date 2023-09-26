using erpl.api.Helpers;
using erpl.common.infrastructure.PropertyMappings;
using erpl.common.infrastructure.TypeHelpers;
using erpl.contracts.ContractRepositories;
using erpl.contracts.V1.Wiretaps;
using erpl.repository.Repositories.Wiretaps;
using erpl.services.V1.Wiretaps;
using Microsoft.Extensions.DependencyInjection;

namespace erpl.api.Configurations;

internal static class Config
{
    internal static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IPropertyMappingService, PropertyMappingService>();
        services.AddSingleton<ITypeHelperService, TypeHelperService>();

        return services;
    }

    internal static IServiceCollection ConfigureContracts(this IServiceCollection services)
    {
        services.ConfigureWiretapContracts();

        return services;
    }

    private static IServiceCollection ConfigureWiretapContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateWiretapProcessor, CreateWiretapProcessor>();
        services.AddScoped<IUpdateWiretapProcessor, UpdateWiretapProcessor>();
        services.AddScoped<IDeleteWiretapProcessor, DeleteWiretapProcessor>();
        services.AddScoped<IGetWiretapByIdProcessor, GetWiretapByIdProcessor>();
        services.AddScoped<IGetWiretapsProcessor, GetWiretapsProcessor>();

        services.AddScoped<IWiretapRepository, WiretapRepository>();
        return services;
    }

    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {
        // Add framework services.
        services.AddControllers(options =>
          {
              options.Filters.Add(typeof(HttpGlobalExceptionFilter));
          })
          // Added for functional tests
          .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
          builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
        });

        return services;
    }
}//Class : Config