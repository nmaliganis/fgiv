using System.Reflection;
using erpl.api.Configurations;
using erpl.api.Configurations.Installers;
using erpl.services.V1.Suspects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace erpl.api;

/// <summary>
/// Startup class for configurations
/// </summary>
public class Startup
{
    /// <summary>
    /// Startup ctor
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="hostEnv"></param>
    public Startup(IConfiguration configuration, IWebHostEnvironment hostEnv)
    {
        this.Configuration = configuration;
        this.HostEnv = hostEnv;
    }

    /// <summary>
    /// IConfiguration
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// IWebHostEnvironment
    /// </summary>
    public IWebHostEnvironment HostEnv { get; }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAspBasicInstaller();

        services.AddSwaggerInstaller();

        services.AddSerilogInstaller(this.Configuration, this.HostEnv);

        services.AddRoutingVersioningInstaller();

        services.AddAutoMapperInstaller();

        services.ConfigureInfrastructure();

        services.ConfigureContracts();

        services.AddCustomMvc();

        services.AddListStartupServicesInstaller();

        services.AddHttpClient();

        services.AddControllers()
            .AddNewtonsoftJson(x =>
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(CreateSuspectProcessor).GetTypeInfo().Assembly,
            typeof(GetSuspectByIdProcessor).GetTypeInfo().Assembly,
            typeof(GetSuspectsProcessor).GetTypeInfo().Assembly,
            typeof(UpdateSuspectProcessor).GetTypeInfo().Assembly,
            typeof(DeleteSuspectProcessor).GetTypeInfo().Assembly
        ));
    }

    /// <summary>
    /// Configure application builder
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsStaging() || env.IsProduction())
        {
            app.UseListStartupServicesInstaller();
            app.UseDeveloperExceptionPage();

            app.UseSwaggerInstaller();
        }
        else
        {
            app.UseHsts();
        }

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
            .AddEnvironmentVariables()
            .Build();

        app.UseRouting();
        app.UseAspBasicInstaller();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
        });

        app.UseRoutingVersioningInstaller();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }

} // Class : Startup