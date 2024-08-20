namespace AITinker.OpenAI.Models;

public class OpenAIConfig {
    public OpenAIConfig() { 
        Defaults = new OpenAISettings();
        Options = new OpenAIOptions();
    }

    public OpenAISettings? Defaults { get; set; }
    public OpenAIOptions? Options { get; set; }
}

public class OpenAISettings {
    public string? ApiKey { get; set; }
    public string? ApiUrl { get; set; }
    public string? Model { get; set; }
    public string? SystemContent { get; set; }
    public double? Temperature { get; set; }
}

public class OpenAIOptions {
    public string[]? ApiUrl { get; set; }
    public string[]? Model { get; set; }
}

