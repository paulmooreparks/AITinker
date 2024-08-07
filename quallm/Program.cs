using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Cliffer;
using Quallm.Cli.Services;

namespace Quallm.Cli;

internal class Program {
    private const string _appDirectoryName = ".quallm";
    private const string _configFileName = "appsettings.json";
    private static readonly string _configEnvFileName = $"appsettings.{0}.json";

    private static readonly string _appDirectory;
    private static readonly string _configFilePath;
    private static readonly string _configEnvFilePath;

    static Program() {
        string environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
        _configEnvFileName = string.Format(_configEnvFileName, environment);

        var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        _appDirectory = Path.Combine(homeDirectory, _appDirectoryName);
        _configFilePath = Path.Combine(_appDirectory, _configFileName);
        _configEnvFilePath = Path.Combine(_appDirectory, _configEnvFileName);
    }

    static async Task<int> Main(string[] args) {
        if (!Directory.Exists(_appDirectory)) {
            Directory.CreateDirectory(_appDirectory);
        }

        var cli = new ClifferBuilder()
            .ConfigureAppConfiguration((configurationBuilder) => {
                configurationBuilder
                    .AddJsonFile(_configFilePath, optional: true, reloadOnChange: true)
                    .AddJsonFile(_configEnvFilePath, optional: true, reloadOnChange: true);
            })
            .ConfigureServices(services => {
                // services.AddSingleton<PersistenceService>();
            })
            .Build();

        ServiceUtility.SetServiceProvider(cli.ServiceProvider);

        ClifferEventHandler.OnExit += () => {
            // var persistenceService = ServiceUtility.GetService<PersistenceService>()!;
            // persistenceService.SaveVariables(variables);
        };

        return await cli.RunAsync(args);
    }
}
