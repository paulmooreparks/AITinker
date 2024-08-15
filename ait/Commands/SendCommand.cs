using Cliffer;
using Newtonsoft.Json.Linq;

using AITinker.OpenAI.Services;

namespace AITinker.Cli.Commands;

[Command("send", "Send a message to the LLM")]
[Argument(typeof(string), "message", "The message to send to the LLM")]
[Option(typeof(bool), "--usage", "Show usage information", aliases: ["-u"])]
internal class SendCommand {
    private readonly OpenAIService _openAIService;
    
    public SendCommand(OpenAIService openAIService) { 
        _openAIService = openAIService;
    }

    public async Task<int> Execute(
        string message,
        [OptionParam("--usage")] bool showUsage
        ) {
        try {
            string? pipedInput = string.Empty;

            if (Console.IsInputRedirected) {
                using (var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding)) {
                    if (reader.Peek() >= 0) {
                        pipedInput = await reader.ReadToEndAsync();
                    }
                }
            }

            message = string.Concat(message, pipedInput);

            if (string.IsNullOrEmpty(message)) {
                Console.WriteLine("No message provided.");
                return Result.Error;
            }

            // Nearly all of this logic is going to move to the individual LLM libraries.
            var response = await _openAIService.SendMessage(message, pipedInput);

            if (string.IsNullOrEmpty(response.Content)) {
                Console.WriteLine("No response from the LLM.");
                return Result.Error;
            }

            Console.WriteLine(response.Content);

            if (showUsage) {
                foreach (var pair in response.ExtraData) {
                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                }
            }

            return Result.Success;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }

        return Result.Error;
    }
}
