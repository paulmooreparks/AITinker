using Cliffer;

using AITinker.Core.Services;
using AITinker.OpenAI.Models;

namespace AITinker.OpenAI.Commands;

[Command("oa-setapikey", "Set the API key for the LLM", aliases: ["oa-sk"], Parent = "openai")]
[Argument(typeof(string), "key", "The API key to set", Arity = ArgumentArity.ExactlyOne)]
internal class SetApiKeyCommand {
    public int Execute(string key, IWriteableSection<OpenAIConfig> configSection) {
        Console.WriteLine($"Setting API key to {key}");
        configSection.Update(config => {
            if (config is null) {                 
                config = new OpenAIConfig();
            }

            config.Settings!.ApiKey = key;
        });

        return Result.Success;
    }
}
