using System.Net.Http.Headers;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using AITinker.Core.Services;
using AITinker.Core.Models;
using AITinker.OpenAI.Models;

namespace AITinker.OpenAI.Services;

public class OpenAIService : ILLMService {
    private readonly IWriteableSection<OpenAIConfig> _config;
    private readonly HttpClient _httpClient;

    public string ApiKey { 
        get {
            return _config?.Value.Settings?.ApiKey ?? string.Empty;
        } 
        set {
            _config.Value.Settings!.ApiKey = value;
        }
    }

    public string ApiUrl {
        get {
            return _config.Value.Settings!?.ApiUrl ?? "https://api.openai.com/v1/chat/completions";
        }
        set {
            _config.Value.Settings!.ApiUrl = value;
        }
    }

    public string Model {
        get {
            return _config.Value.Settings?.Model ?? "gpt-4o-mini";
        }
        set {
            _config.Value.Settings!.Model = value;
        }
    }

    public string SystemContent {
        get {
            return _config.Value.Settings?.SystemContent ?? "You are a helpful assistant.";
        }
        set {
            _config.Value.Settings!.SystemContent = value;
        }
    }

    public double Temperature {
        get {
            return _config.Value.Settings?.Temperature ?? 0.5;
        }
        set {
            _config.Value.Settings!.Temperature = value;
        }
    }

    public OpenAIService(IWriteableSection<OpenAIConfig> config) {
        _config = config;

        if (_config.Value.Settings is null) {
            _config.Value.Settings = new OpenAIConfig.SettingsModel();
        }

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
    }

    public void SaveSettings() {
        _config.Update(config => { });
    }

    public async Task<LLMResponse> SendMessage(string message, string extra) {
        var messageList = new List<object>();

        if (!string.IsNullOrEmpty(SystemContent)) {
            var systemMessage = new {
                role = "system",
                content = SystemContent
            };

            messageList.Add(systemMessage);
        }

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
            model = Model,
            messages = messageList.ToArray(),
            temperature = Temperature
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(ApiUrl, content);

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
