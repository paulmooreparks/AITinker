using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.FileProviders;

namespace Quallm.Cli.Services;

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
        string file) {
        _options = options;
        _configuration = configuration;
        _fileProvider = ServiceUtility.GetService<IFileProvider>()!;
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