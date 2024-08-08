using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Quallm.Cli.Services;

namespace Quallm.Cli.Extensions;

public static class ServiceCollectionExtensions {
    public static void ConfigureWritable<T>(
        this IServiceCollection services,
        IConfigurationSection section,
        string file = "appsettings.json") where T : class, new() 
    {
        services.Configure<T>(section);
        services.AddTransient<IWriteableSection<T>>(provider => {
            var configuration = provider.GetService<IConfiguration>();
            var configurationRoot = configuration as IConfigurationRoot;

            if (configurationRoot is null) {
                throw new NullReferenceException(nameof(configurationRoot));
            }

            var options = provider.GetService<IOptionsMonitor<T>>();

            if (options is null) {
                throw new NullReferenceException(nameof(options));
            }

            return new WriteableSection<T>(options, configurationRoot, section.Key, file);
        });
    }
}