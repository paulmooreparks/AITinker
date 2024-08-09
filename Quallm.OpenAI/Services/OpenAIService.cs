using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Quallm.OpenAI.Models;

namespace Quallm.OpenAI.Services;

public class OpenAIService {
    private readonly OpenAIConfig _config;
    private readonly HttpClient _httpClient;

    public OpenAIService(OpenAIConfig config) {
        _config = config;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.ApiKey);
    }

    public async Task<string> SendMessage(string message) {
        var prompt = new {
            role = "user",
            content = message
        };

        var requestBody = new {
            model = "gpt-4o-mini", // Specify the model you want to use
            messages = new[] { prompt },
            temperature = 0.7
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_config.ApiUrl, content);

        if (response.IsSuccessStatusCode) {
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        else {
            throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
        }
    }
}
