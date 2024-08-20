using Cliffer;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

using AITinker.Cli.Services;
using AITinker.OpenAI.Extensions;
using AITinker.Core.Extensions;

namespace AITinker.Cli;

internal class AitProgram {

    static async Task<int> Main(string[] args) {
        var clifferBuilder = new ClifferBuilder();

        clifferBuilder.ConfigureAppConfiguration((configurationBuilder) => {
            configurationBuilder.SetFileProvider(AITinker.Core.Configuration.FileProvider);
            configurationBuilder.AddJsonFile(AITinker.Core.Configuration.ConfigFileName, optional: true, reloadOnChange: true);
        });

        var configuration = clifferBuilder.BuildConfiguration();

        clifferBuilder.ConfigureServices((services) => {
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<IFileProvider>(AITinker.Core.Configuration.FileProvider);
            services.AddLLMServices(configuration);
            services.AddOpenAIServices(configuration);
        });

        var cli = clifferBuilder
            .Build((configuration, rootCommand, serviceProvider) => {
                clifferBuilder.AddOpenAICommands(configuration, rootCommand, serviceProvider);
            });

        ServiceUtility.SetServiceProvider(cli.ServiceProvider);

        ClifferEventHandler.OnExit += () => {
        };

        return await cli.RunAsync(args);
    }
}

