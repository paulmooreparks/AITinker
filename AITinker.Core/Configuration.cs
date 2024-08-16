using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

using AITinker.Core.Services;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AITinker.Core;

public static class Configuration {
    private const string _appDirectoryName = ".aitinker";
    private const string _configFileName = "appsettings.json";

    private static readonly string _appDirectory;
    private static readonly string _configFilePath;
    private static readonly IFileProvider _fileProvider;

    static Configuration() {
        string environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        _appDirectory = Path.Combine(homeDirectory, AITinker.Core.Configuration.AppDirectoryName);
        _configFilePath = Path.Combine(_appDirectory, AITinker.Core.Configuration.ConfigFileName);

        if (!Directory.Exists(_appDirectory)) {
            Directory.CreateDirectory(_appDirectory);
        }

        _fileProvider = new PhysicalFileProvider(_appDirectory, Microsoft.Extensions.FileProviders.Physical.ExclusionFilters.None);

        if (!File.Exists(AITinker.Core.Configuration.ConfigFilePath)) {
            var emptyObject = new JObject();
            File.WriteAllText(AITinker.Core.Configuration.ConfigFilePath, emptyObject.ToString(Formatting.Indented));
        }
    }

    public static string AppDirectoryName => _appDirectoryName;
    public static string ConfigFileName => _configFileName;
    public static string AppDirectory => _appDirectory; 
    public static string ConfigFilePath => _configFilePath;
    public static IFileProvider FileProvider => _fileProvider;
}
