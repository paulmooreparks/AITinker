using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AITinker.Core.Attributes;
using AITinker.Core.Extensions;
using AITinker.OpenAI.Models;
using AITinker.OpenAI.Services;

namespace AITinker.OpenAI.Extensions;

public static class ServiceCollectionExtensions {
    private const string _tempKey = "Configurations:Test1:Settings";
    private const string _key = "Kits:OpenAI";
    private const string _defaultsKey = "Kits:OpenAI:Defaults";
    private const string _optionsKey = "Kits:OpenAI:Options";

    [RegisterServices]
    public static IServiceCollection AddOpenAIServices(this IServiceCollection services, IConfiguration configuration) {
        var openAISettings = configuration.GetSection(_tempKey).Get<OpenAISettings>();
        services.ConfigureWritable<OpenAISettings>(configuration.GetSection(_tempKey), AITinker.Core.Configuration.ConfigFileName);

        if (openAISettings is null) {
            openAISettings = new OpenAISettings();
            configuration.GetSection(_tempKey).Bind(openAISettings);
        }

        services.AddSingleton(openAISettings);

        var openAIConfig = configuration.GetSection(_key).Get<OpenAIConfig>();
        services.ConfigureWritable<OpenAIConfig>(configuration.GetSection(_key), AITinker.Core.Configuration.ConfigFileName);

        if (openAIConfig is null) {
            openAIConfig = new OpenAIConfig();
            configuration.GetSection(_key).Bind(openAIConfig);
        }

        services.AddSingleton(openAIConfig);

        var openAIDefaults = configuration.GetSection(_defaultsKey).Get<OpenAIDefaults>();

        if (openAIDefaults is null) {
            openAIDefaults = new OpenAIDefaults();
            configuration.GetSection(_defaultsKey).Bind(openAIDefaults);
        }

        services.AddSingleton(openAIDefaults);

        var openAIOptions = configuration.GetSection(_optionsKey).Get<OpenAIOptions>();

        if (openAIOptions is null) {
            openAIOptions = new OpenAIOptions();
            configuration.GetSection(_optionsKey).Bind(openAIOptions);
        }

        services.AddSingleton(openAIOptions);

        services.AddSingleton<OpenAIService>();

        return services;
    }
}
