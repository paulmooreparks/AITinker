using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AITinker.Core.Attributes;
using AITinker.Core.Extensions;
using AITinker.OpenAI.Models;
using AITinker.OpenAI.Services;

namespace AITinker.OpenAI.Extensions;

public static class ServiceCollectionExtensions {
    private const string _key = "LLMConfig:OpenAI";

    [RegisterServices]
    public static IServiceCollection AddOpenAIServices(this IServiceCollection services, IConfiguration configuration) {
        var openAIConfig = configuration.GetSection(_key).Get<OpenAIConfig>();
        services.ConfigureWritable<OpenAIConfig>(configuration.GetSection(_key), "appsettings.json");

        if (openAIConfig is null) {
            openAIConfig = new OpenAIConfig();
            configuration.GetSection(_key).Bind(openAIConfig);
        }

        services.AddSingleton(openAIConfig);
        services.AddSingleton<OpenAIService>();

        return services;
    }
}
