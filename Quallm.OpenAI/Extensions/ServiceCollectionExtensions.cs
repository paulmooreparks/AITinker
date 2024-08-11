using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quallm.OpenAI.Models;
using Quallm.ConfigUtils.Extensions;
using Quallm.OpenAI.Services;
using Microsoft.Extensions.Options;
using Quallm.ConfigUtils.Services;

namespace Quallm.OpenAI.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddOpenAIServices(this IServiceCollection services, IConfiguration configuration, string configFileName = "appsettings.json") {
        var chatGPTConfig = configuration.GetSection("ChatGPT").Get<OpenAIConfig>();
        services.ConfigureWritable<OpenAIConfig>(configuration.GetSection("ChatGPT"), configFileName);

        if (chatGPTConfig is null) {
            chatGPTConfig = new OpenAIConfig();
            configuration.GetSection("ChatGPT").Bind(chatGPTConfig);
        }

        services.AddSingleton(chatGPTConfig);
        services.AddSingleton<OpenAIService>();

        return services;
    }
}
