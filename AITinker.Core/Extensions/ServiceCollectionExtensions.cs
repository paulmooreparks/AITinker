using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

using AITinker.Core.Attributes;
using AITinker.Core.Models;
using AITinker.Core.Services;

namespace AITinker.Core.Extensions;

public static class ServiceCollectionExtensions {
    private const string _configurationsKey = "Configurations";
    private const string _kitsKey = "Kits";

    public static IServiceCollection AddLLMServices(this IServiceCollection services, IConfiguration configuration) {
        if (services is null) {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration is null) {
            throw new ArgumentNullException(nameof(services));
        }

        var configurationsSection = configuration.GetSection(_configurationsKey);
        var configurationsDict = new Dictionary<string, KitConfiguration>();

        foreach (var child in configurationsSection.GetChildren()) {
            var config = child.Get<KitConfiguration>();
            if (config != null) {
                configurationsDict.Add(child.Key, config);
            }
        }

        var aitConfigurations = new Configurations {
            Table = configurationsDict
        };

        if (aitConfigurations is not null) {
            services.AddSingleton<Configurations>(aitConfigurations);
        }

        services.ConfigureWritable<Configurations>(configuration.GetSection(_configurationsKey), AITinker.Core.Configuration.ConfigFileName);
        // services.ConfigureWritable<Kits>(configuration.GetSection(_kitsKey), AITinker.Core.KitConfiguration.ConfigFileName);

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
