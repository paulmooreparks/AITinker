using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Quallm.ConfigUtils.Services;

public class WriteableSection<T> : IWriteableSection<T> where T : class, new() {
    private readonly IOptionsMonitor<T> _options;
    private readonly IConfigurationRoot _configuration;
    private readonly string _section;
    private readonly string _file;
    private readonly IFileProvider _fileProvider;

    public WriteableSection(
        IOptionsMonitor<T> options,
        IConfigurationRoot configuration,
        string section,
        string file,
        IFileProvider? fileProvider = null,
        string? appDirectory = null
        ) {
        _options = options;
        _configuration = configuration;

        if (fileProvider is not null) {
            _fileProvider = fileProvider;
        }
        else if (appDirectory is not null) {
            _fileProvider = new PhysicalFileProvider(appDirectory, Microsoft.Extensions.FileProviders.Physical.ExclusionFilters.None);
        }
        else {
            _fileProvider = new PhysicalFileProvider(Environment.CurrentDirectory, Microsoft.Extensions.FileProviders.Physical.ExclusionFilters.None);
        }

        _section = section;
        _file = file;
    }

    public T Value => _options.CurrentValue;
    public T Get(string name) => _options.Get(name);

    public void Update(Action<T> applyChanges) {

        var fileInfo = _fileProvider.GetFileInfo(_file);

        if (fileInfo is null) {
            throw new NullReferenceException(nameof(fileInfo));
        }

        var physicalPath = fileInfo.PhysicalPath;

        if (physicalPath is null) {
            throw new NullReferenceException(nameof(physicalPath));
        }

        var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath));

        if (jObject is null) {
            throw new NullReferenceException(nameof(jObject));
        }

        var sectionObject = jObject.TryGetValue(_section, out JToken? section) ? JsonConvert.DeserializeObject<T>(section.ToString()) : Value ?? new T();

        if (sectionObject is null) {
            throw new NullReferenceException(nameof(sectionObject));
        }

        applyChanges(sectionObject);

        jObject[_section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
        File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        _configuration.Reload();
    }
}