using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quallm.OpenAI.Models;
using Quallm.ConfigUtils.Extensions;
using Quallm.OpenAI.Services;

namespace Quallm.OpenAI.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddOpenAIServices(this IServiceCollection services, IConfiguration configuration, string configFileName = "appsettings.json") {
        var chatGPTConfig = configuration.GetSection("ChatGPT").Get<OpenAIConfig>() ?? new OpenAIConfig();
        services.AddSingleton(chatGPTConfig);
        services.ConfigureWritable<OpenAIConfig>(configuration.GetSection("ChatGPT"), configFileName);
        services.AddSingleton<OpenAIService>();
        return services;
    }
}
