using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

using AITinker.Core.Attributes;
using AITinker.Core.Services;

namespace AITinker.Core.Extensions;

public static class ServiceCollectionExtensions {
    private const string _configurationsKey = "Configurations";
    private const string _kitsKey = "Kits";

    public static IServiceCollection AddLLMServices(this IServiceCollection services, IConfiguration configuration, Assembly assembly) {
        if (services is null) {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration is null) {
            throw new ArgumentNullException(nameof(services));
        }

        if (assembly is null) {
            throw new ArgumentNullException(nameof(assembly));
        }

        var types = assembly.GetTypes();

        foreach (var type in types) {
            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(m => m.GetCustomAttribute<RegisterServicesAttribute>() != null);

            foreach (var method in methods) {
                method.Invoke(null, [services, configuration]);
            }
        }

        // services.ConfigureWritable<Configurations>(configuration.GetSection(_configurationsKey), AITinker.Core.Configuration.ConfigFileName);
        // services.ConfigureWritable<Kits>(configuration.GetSection(_kitsKey), AITinker.Core.Configuration.ConfigFileName);

        return services;
    }


    public static void ConfigureWritable<T>(
        this IServiceCollection services,
        IConfigurationSection section,
        string file = AITinker.Core.Configuration.ConfigFileName,
        IFileProvider? fileProvider = null,
        string? appDirectory = null) where T : class, new() 
    {
        services.Configure<T>(section);
        services.AddTransient<IWriteableSection<T>>(provider => {
            var configuration = provider.GetService<IConfiguration>();
            var fileProvider = provider.GetService<IFileProvider>();
            var configurationRoot = configuration as IConfigurationRoot;

            if (configurationRoot is null) {
                throw new NullReferenceException(nameof(configurationRoot));
            }

            var options = provider.GetService<IOptionsMonitor<T>>();

            if (options is null) {
                throw new NullReferenceException(nameof(options));
            }

            return new WriteableSection<T>(options, configurationRoot, section.Key, file, fileProvider);
        });
    }
}
