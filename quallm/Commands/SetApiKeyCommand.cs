using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cliffer;

using Quallm.Cli.Models;
using Quallm.Cli.Services;

namespace Quallm.Cli.Commands;

[Command("setapikey", "Set the API key for the LLM")]
[Option(typeof(string), "--key", "The API key to set")]
internal class SetApiKeyCommand {
    public int Execute(string key, IWriteableSection<ChatGPTConfig> configSection) {
        Console.WriteLine($"Setting API key to {key}");
        configSection.Update(config => config.ApiKey = key);
        return Result.Success;
    }
}
