using System.Net.Http.Headers;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using AITinker.Core.Services;
using AITinker.Core.Models;
using AITinker.OpenAI.Models;

namespace AITinker.OpenAI.Services;

public class OpenAIService : ILLMService {
    private readonly OpenAIConfig? _config;
    private readonly HttpClient _httpClient;

    public OpenAIService(OpenAIConfig config) {
        _config = config;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config?.Settings?.ApiKey);
    }

    public async Task<LLMResponse> SendMessage(string message, string extra) {
        var messageList = new List<object>();

        /* Move this out to ait */
        var systemMessage = new {
            role = "system",
            content = "You are a helpful assistant being called from a command-line application. Use plain text as much as possible rather than formats such as Markdown."
        };

        messageList.Add(systemMessage);

        var prompt = new {
            role = "user",
            content = message
        };

        messageList.Add(prompt);

        if (!string.IsNullOrEmpty(extra)) {
            var extraMessage = new {
                role = "user",
                content = extra
            };

            messageList.Add(extraMessage);
        }

        var requestBody = new {
            model = _config?.Settings?.Model ?? "gpt-4o-mini",
            messages = messageList.ToArray(),
            temperature = 0.5
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_config?.Settings?.ApiUrl, content);

        if (response.IsSuccessStatusCode) {
            var llmResponse = new LLMResponse();
            var jsonResponse = JObject.Parse(await response.Content.ReadAsStringAsync());

            var choices = jsonResponse["choices"];

            if (choices is not null) {
                llmResponse.Content = choices[0]?["message"]?["content"]?.ToString() ?? string.Empty;

                if (choices.Count() > 1) {
                    for (var i = 1; i < choices.Count(); i++) {
                        llmResponse.ExtraData[$"choice_{i}"] = choices[i]?["message"]?["content"]?.ToString() ?? string.Empty;
                    }
                }
            }

            if (jsonResponse.ContainsKey("usage")) {
                var usage = jsonResponse["usage"];

                if (usage is not null) {
                    llmResponse.ExtraData["prompt_tokens"] = usage["prompt_tokens"]?.ToString() ?? string.Empty;
                    llmResponse.ExtraData["completion_tokens"] = usage["completion_tokens"]?.ToString() ?? string.Empty;
                    llmResponse.ExtraData["total_tokens"] = usage["total_tokens"]?.ToString() ?? string.Empty;
                }
            }

            return llmResponse;
        }
        else {
            throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
        }
    }
}
