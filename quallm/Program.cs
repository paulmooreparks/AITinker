using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Cliffer;
using Quallm.Cli.Services;
using Quallm.Cli.Models;
using Quallm.Cli.Extensions;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Quallm.Cli;

internal class Program {
    private const string _appDirectoryName = ".quallm";
    private const string _configFileName = "appsettings.json";
    private static readonly string _configEnvFileName = "appsettings.{0}.json";

    private static readonly string _appDirectory;
    private static readonly string _configFilePath;
    private static readonly string _configEnvFilePath;
    private static readonly IFileProvider _fileProvider; 


    static Program() {
        string environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
        _configEnvFileName = string.Format(_configEnvFileName, environment);

        var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        _appDirectory = Path.Combine(homeDirectory, _appDirectoryName);
        _configFilePath = Path.Combine(_appDirectory, _configFileName);
        _configEnvFilePath = Path.Combine(_appDirectory, _configEnvFileName);

        if (!Directory.Exists(_appDirectory)) {
            Directory.CreateDirectory(_appDirectory);
        }

        _fileProvider = new PhysicalFileProvider(_appDirectory, Microsoft.Extensions.FileProviders.Physical.ExclusionFilters.None);
    }

    static async Task<int> Main(string[] args) {
        var clifferBuilder = new ClifferBuilder();

        if (!File.Exists(_configFilePath)) {
            var emptyObject = new JObject();
            File.WriteAllText(_configFilePath, emptyObject.ToString(Formatting.Indented));
        }

        clifferBuilder.ConfigureAppConfiguration((configurationBuilder) => {
            configurationBuilder.SetFileProvider(_fileProvider);
            configurationBuilder.AddJsonFile(_configFileName, optional: true, reloadOnChange: true);
        });

        var configuration = clifferBuilder.BuildConfiguration();

        clifferBuilder.ConfigureServices((services) => {
            var chatGPTConfig = configuration.GetSection("ChatGPT").Get<ChatGPTConfig>() ?? new ChatGPTConfig();
            services.AddSingleton(chatGPTConfig);
            services.ConfigureWritable<ChatGPTConfig>(configuration.GetSection("ChatGPT"), _configFileName);
            services.AddSingleton<ChatGPTService>();
            services.AddSingleton<IFileProvider>(_fileProvider);
        });


        var cli = clifferBuilder.Build();

        ServiceUtility.SetServiceProvider(cli.ServiceProvider);

        ClifferEventHandler.OnExit += () => {
            // var persistenceService = ServiceUtility.GetService<PersistenceService>()!;
            // persistenceService.SaveVariables(variables);
        };

        return await cli.RunAsync(args);
    }
}

