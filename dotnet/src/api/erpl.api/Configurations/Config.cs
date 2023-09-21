using erpl.api.Helpers;
using erpl.common.infrastructure.PropertyMappings;
using erpl.common.infrastructure.TypeHelpers;
using erpl.contracts.ContractRepositories;
using erpl.contracts.V1.Suspects;
using erpl.repository.Repositories.Suspects;
using erpl.services.V1.Suspects;
using Microsoft.Extensions.Configuration;
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
        services.ConfigureSuspectContracts();

        return services;
    }

    private static IServiceCollection ConfigureSuspectContracts(this IServiceCollection services)
    {
        services.AddScoped<ICreateSuspectProcessor, CreateSuspectProcessor>();
        services.AddScoped<IUpdateSuspectProcessor, UpdateSuspectProcessor>();
        services.AddScoped<IDeleteSuspectProcessor, DeleteSuspectProcessor>();
        services.AddScoped<IGetSuspectByIdProcessor, GetSuspectByIdProcessor>();
        services.AddScoped<IGetSuspectsProcessor, GetSuspectsProcessor>();

        services.AddScoped<ISuspectRepository, SuspectRepository>();
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