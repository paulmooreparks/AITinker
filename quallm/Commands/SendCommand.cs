using Cliffer;
using Newtonsoft.Json.Linq;

using Quallm.OpenAI.Services;

namespace Quallm.Cli.Commands;

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
            var response = await _openAIService.SendMessage(message);

            // Parse the response
            var jsonResponse = JObject.Parse(response);
            var choices = jsonResponse["choices"];

            if (choices is not null) {
                string? content = string.Empty;

                if (choices.Count() > 1) {
                    int choiceIndex = 1;
                    foreach (var choice in choices) {
                        Console.WriteLine($"Reponse choice {choiceIndex}:");
                        if (choice is not null) {
                            content = choice["message"]?["content"]?.ToString();

                            if (!string.IsNullOrEmpty(content)) {
                                Console.WriteLine(content);
                            }
                            else {
                                Console.WriteLine("No content found in the response.");
                            }
                        }
                    }
                }
                else {
                    content = choices[0]?["message"]?["content"]?.ToString();

                    if (!string.IsNullOrEmpty(content)) {
                        Console.WriteLine(content);
                    }
                    else {
                        Console.WriteLine("No content found in the response.");
                    }
                }

            }
            else {
                Console.WriteLine("No content found in the response.");
            }

            if (showUsage) {
                var promptTokens = jsonResponse["usage"]?["prompt_tokens"]?.ToString();
                var completionTokens = jsonResponse["usage"]?["completion_tokens"]?.ToString();
                var totalTokens = jsonResponse["usage"]?["total_tokens"]?.ToString();

                if (promptTokens != null && completionTokens != null && totalTokens != null) {
                    Console.WriteLine($"Prompt tokens: {promptTokens}");
                    Console.WriteLine($"Completion tokens: {completionTokens}");
                    Console.WriteLine($"Total tokens: {totalTokens}");
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
