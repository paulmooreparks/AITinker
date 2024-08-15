using System.CommandLine;
using System.Reflection;

using Cliffer;

using Microsoft.Extensions.Configuration;

using AITinker.OpenAI.Services;

namespace AITinker.OpenAI.Extensions;

public static class ClifferBuilderExtensions {
    public static IClifferBuilder AddOpenAICommands(this IClifferBuilder clifferBuilder, IConfiguration configuration, RootCommand rootCommand, IServiceProvider serviceProvider) {
        var assembly = Assembly.GetExecutingAssembly();
        clifferBuilder.AddCommands(assembly);
        ServiceUtility.SetServiceProvider(serviceProvider);
        return new ClifferBuilder(); 
    }
}
